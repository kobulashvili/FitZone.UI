using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;
using FitZone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitZone.Infrastructure.Repositories;

public class WorkoutSessionRepository : IWorkoutSessionRepository
{
    private readonly FitZoneDbContext _context;

    public WorkoutSessionRepository(FitZoneDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutSession?> GetByIdAsync(int id)
    {
        return await _context.WorkoutSessions
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<WorkoutSession?> GetByIdWithBookingsAsync(int id)
    {
        return await _context.WorkoutSessions
            .Include(s => s.Bookings)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<WorkoutSession>> GetAllAsync()
    {
        return await _context.WorkoutSessions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<WorkoutSession>> GetByTrainerIdAsync(int trainerId)
    {
        return await _context.WorkoutSessions
            .Where(s => s.TrainerId == trainerId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(WorkoutSession session)
    {
        await _context.WorkoutSessions.AddAsync(session);
    }

    public Task UpdateAsync(WorkoutSession session)
    {
        _context.WorkoutSessions.Update(session);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(WorkoutSession session)
    {
        _context.WorkoutSessions.Remove(session);
        return Task.CompletedTask;
    }
}