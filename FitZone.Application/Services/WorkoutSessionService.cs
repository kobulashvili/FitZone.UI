
using AutoMapper;
using FitZone.Application.DTOs.WorkoutSessions;
using FitZone.Application.Interfaces.Services;
using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;

namespace FitZone.Application.Services;

public class WorkoutSessionService : IWorkoutSessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WorkoutSessionService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WorkoutSessionResponseDto> CreateAsync(
        CreateWorkoutSessionDto dto)
    {
        var trainer = await _unitOfWork.Trainers
            .GetByIdAsync(dto.TrainerId);

        if (trainer is null)
        {
            throw new InvalidOperationException("Trainer not found.");
        }

        var trainerSessions = await _unitOfWork.WorkoutSessions
            .GetByTrainerIdAsync(dto.TrainerId);

        DateTime newSessionEnd =
            dto.StartTime.AddMinutes(dto.DurationMinutes);

        bool hasOverlap = trainerSessions.Any(s =>
            !s.IsCancelled &&
            dto.StartTime < s.StartTime.AddMinutes(s.DurationMinutes) &&
            newSessionEnd > s.StartTime);

        if (hasOverlap)
        {
            throw new InvalidOperationException(
                "Trainer already has a session at this time.");
        }

        var session = new WorkoutSession(
            dto.TrainerId,
            dto.WorkoutType,
            dto.StartTime,
            dto.DurationMinutes,
            dto.Capacity,
            dto.IsPremiumOnly
        );

        await _unitOfWork.WorkoutSessions.AddAsync(session);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<WorkoutSessionResponseDto>(session);
    }

    public async Task<WorkoutSessionResponseDto?> GetByIdAsync(
        int sessionId)
    {
        var session = await _unitOfWork.WorkoutSessions
            .GetByIdWithBookingsAsync(sessionId);

        if (session is null)
        {
            return null;
        }

        return _mapper.Map<WorkoutSessionResponseDto>(session);
    }

    public async Task<List<WorkoutSessionResponseDto>> GetAllAsync()
    {
        var sessions = await _unitOfWork.WorkoutSessions.GetAllAsync();

        return _mapper.Map<List<WorkoutSessionResponseDto>>(sessions);
    }

    public async Task<List<WorkoutSessionResponseDto>> GetByTrainerIdAsync(
        int trainerId)
    {
        var sessions = await _unitOfWork.WorkoutSessions
            .GetByTrainerIdAsync(trainerId);

        return _mapper.Map<List<WorkoutSessionResponseDto>>(sessions);
    }

    public async Task UpdateAsync(
        int sessionId,
        UpdateWorkoutSessionDto dto)
    {
        var session = await _unitOfWork.WorkoutSessions
            .GetByIdWithBookingsAsync(sessionId);

        if (session is null)
        {
            throw new InvalidOperationException(
                "Workout session not found.");
        }

        var trainer = await _unitOfWork.Trainers
            .GetByIdAsync(dto.TrainerId);

        if (trainer is null)
        {
            throw new InvalidOperationException("Trainer not found.");
        }

        var trainerSessions = await _unitOfWork.WorkoutSessions
            .GetByTrainerIdAsync(dto.TrainerId);

        DateTime newSessionEnd =
            dto.StartTime.AddMinutes(dto.DurationMinutes);

        bool hasOverlap = trainerSessions.Any(s =>
            s.Id != sessionId &&
            !s.IsCancelled &&
            dto.StartTime < s.StartTime.AddMinutes(s.DurationMinutes) &&
            newSessionEnd > s.StartTime);

        if (hasOverlap)
        {
            throw new InvalidOperationException(
                "Trainer already has a session at this time.");
        }

        session.UpdateDetails(
            dto.TrainerId,
            dto.WorkoutType,
            dto.StartTime,
            dto.DurationMinutes,
            dto.Capacity,
            dto.IsPremiumOnly
        );

        await _unitOfWork.WorkoutSessions.UpdateAsync(session);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CancelAsync(int sessionId)
    {
        var session = await _unitOfWork.WorkoutSessions
            .GetByIdWithBookingsAsync(sessionId);

        if (session is null)
        {
            throw new InvalidOperationException(
                "Workout session not found.");
        }

        session.CancelSession();

        await _unitOfWork.WorkoutSessions.UpdateAsync(session);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int sessionId)
    {
        var session = await _unitOfWork.WorkoutSessions
            .GetByIdWithBookingsAsync(sessionId);

        if (session is null)
        {
            throw new InvalidOperationException(
                "Workout session not found.");
        }

        if (session.Bookings.Any())
        {
            throw new InvalidOperationException(
                "Cannot delete a session that has bookings. Cancel it instead.");
        }

        await _unitOfWork.WorkoutSessions.DeleteAsync(session);

        await _unitOfWork.SaveChangesAsync();
    }
}
