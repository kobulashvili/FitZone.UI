using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Interfaces;

public interface IWorkoutSessionRepository
{
    Task<WorkoutSession?> GetByIdAsync(int id);

    Task<WorkoutSession?> GetByIdWithBookingsAsync(int id);

    Task<List<WorkoutSession>> GetAllAsync();

    Task<List<WorkoutSession>> GetByTrainerIdAsync(int trainerId);

    Task AddAsync(WorkoutSession session);

    Task UpdateAsync(WorkoutSession session);

    Task DeleteAsync(WorkoutSession session);
}