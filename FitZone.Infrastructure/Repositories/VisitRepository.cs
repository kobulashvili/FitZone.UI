using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;
using FitZone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitZone.Infrastructure.Repositories;

public class VisitRepository : IVisitRepository
{
    private readonly FitZoneDbContext _context;

    public VisitRepository(FitZoneDbContext context)
    {
        _context = context;
    }

    public async Task<Visit?> GetByIdAsync(int id)
    {
        return await _context.Visits
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<Visit>> GetByMemberIdAsync(int memberId)
    {
        return await _context.Visits
            .Where(v => v.MemberId == memberId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Visit visit)
    {
        await _context.Visits.AddAsync(visit);
    }

    public async Task<List<Visit>> GetAllAsync()
    {
        return await _context.Visits
            .AsNoTracking()
            .ToListAsync();
    }
}