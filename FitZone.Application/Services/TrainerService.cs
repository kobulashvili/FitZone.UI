
using System.Security.Cryptography;
using AutoMapper;
using FitZone.Application.DTOs.Trainers;
using FitZone.Application.Interfaces.Services;
using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;

namespace FitZone.Application.Services;

public class TrainerService : ITrainerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TrainerService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainerResponseDto> CreateAsync(
        CreateTrainerDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password) ||
            string.IsNullOrWhiteSpace(dto.Specialization))
        {
            throw new ArgumentException("All fields are required.");
        }

        string email = dto.Email.Trim().ToLowerInvariant();

        var existingMember = await _unitOfWork.Members
            .GetByEmailAsync(email);

        if (existingMember is not null)
        {
            throw new InvalidOperationException(
                "This email is already registered.");
        }

        var existingTrainers = await _unitOfWork.Trainers.GetAllAsync();

        if (existingTrainers.Any(t =>
            t.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "This email is already registered.");
        }

        string passwordHash = HashPassword(dto.Password);

        var trainer = new Trainer(
            dto.FullName.Trim(),
            email,
            passwordHash,
            dto.Specialization.Trim()
        );

        await _unitOfWork.Trainers.AddAsync(trainer);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TrainerResponseDto>(trainer);
    }

    public async Task<TrainerResponseDto?> GetByIdAsync(int trainerId)
    {
        var trainer = await _unitOfWork.Trainers
            .GetByIdAsync(trainerId);

        if (trainer is null)
        {
            return null;
        }

        return _mapper.Map<TrainerResponseDto>(trainer);
    }

    public async Task<List<TrainerResponseDto>> GetAllAsync()
    {
        var trainers = await _unitOfWork.Trainers.GetAllAsync();

        return _mapper.Map<List<TrainerResponseDto>>(trainers);
    }

    public async Task UpdateAsync(
        int trainerId,
        UpdateTrainerDto dto)
    {
        var trainer = await _unitOfWork.Trainers
            .GetByIdAsync(trainerId);

        if (trainer is null)
        {
            throw new InvalidOperationException("Trainer not found.");
        }

        trainer.UpdateDetails(
            dto.FullName,
            dto.Specialization
        );

        await _unitOfWork.Trainers.UpdateAsync(trainer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int trainerId)
    {
        var trainer = await _unitOfWork.Trainers
            .GetByIdAsync(trainerId);

        if (trainer is null)
        {
            throw new InvalidOperationException("Trainer not found.");
        }

        await _unitOfWork.Trainers.DeleteAsync(trainer);

        await _unitOfWork.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
        const int iterations = 100_000;

        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            32
        );

        return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}
