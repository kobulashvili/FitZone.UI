using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;
using FitZone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitZone.Infrastructure.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private readonly FitZoneDbContext _context;

    public TrainerRepository(FitZoneDbContext context)
    {
        _context = context;
    }

    public async Task<Trainer?> GetByIdAsync(int id)
    {
        return await _context.Trainers
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Trainer>> GetAllAsync()
    {
        return await _context.Trainers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Trainer trainer)
    {
        await _context.Trainers.AddAsync(trainer);
    }

    public Task UpdateAsync(Trainer trainer)
    {
        _context.Trainers.Update(trainer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Trainer trainer)
    {
        _context.Trainers.Remove(trainer);
        return Task.CompletedTask;
    }
}