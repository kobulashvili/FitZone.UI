
using AutoMapper;
using FitZone.Application.DTOs.Members;
using FitZone.Application.Interfaces.Services;
using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;
using System.Security.Cryptography;

namespace FitZone.Application.Services;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MemberService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MemberResponseDto> RegisterAsync(
        RegisterMemberDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new ArgumentException("All fields are required.");
        }

        string email = dto.Email.Trim().ToLowerInvariant();

        bool emailExists = await _unitOfWork.Members
            .ExistsByEmailAsync(email);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "A member with this email already exists.");
        }

        string passwordHash = HashPassword(dto.Password);

        var member = new Member(
            dto.FullName.Trim(),
            email,
            passwordHash
        );

        await _unitOfWork.Members.AddAsync(member);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MemberResponseDto>(member);
    }

    public async Task<MemberResponseDto?> LoginAsync(
        LoginMemberDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return null;
        }

        string email = dto.Email.Trim().ToLowerInvariant();

        var member = await _unitOfWork.Members
            .GetByEmailAsync(email);

        if (member is null)
        {
            return null;
        }

        bool passwordIsValid = VerifyPassword(
            dto.Password,
            member.PasswordHash
        );

        if (!passwordIsValid)
        {
            return null;
        }

        return _mapper.Map<MemberResponseDto>(member);
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

    private static bool VerifyPassword(
        string password,
        string storedHash)
    {
        string[] parts = storedHash.Split('.');

        if (parts.Length != 3 ||
            !int.TryParse(parts[0], out int iterations))
        {
            return false;
        }

        try
        {
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] originalHash = Convert.FromBase64String(parts[2]);

            byte[] newHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                originalHash.Length
            );

            return CryptographicOperations.FixedTimeEquals(
                originalHash,
                newHash
            );
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
