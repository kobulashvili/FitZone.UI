using FitZone.Application.DTOs.WorkoutSessions;

namespace FitZone.Application.Interfaces.Services;

public interface IWorkoutSessionService
{
    Task<WorkoutSessionResponseDto> CreateAsync(CreateWorkoutSessionDto dto);

    Task<WorkoutSessionResponseDto?> GetByIdAsync(int sessionId);

    Task<List<WorkoutSessionResponseDto>> GetAllAsync();

    Task<List<WorkoutSessionResponseDto>> GetByTrainerIdAsync(int trainerId);

    Task UpdateAsync(int sessionId, UpdateWorkoutSessionDto dto);

    Task CancelAsync(int sessionId);

    Task DeleteAsync(int sessionId);
}