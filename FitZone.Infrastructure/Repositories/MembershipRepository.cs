using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;
using FitZone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitZone.Infrastructure.Repositories;

public class MembershipRepository : IMembershipRepository
{
    private readonly FitZoneDbContext _context;

    public MembershipRepository(FitZoneDbContext context)
    {
        _context = context;
    }

    public async Task<Membership?> GetByIdAsync(int id)
    {
        return await _context.Memberships
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Membership>> GetByMemberIdAsync(int memberId)
    {
        return await _context.Memberships
            .Where(m => m.MemberId == memberId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Membership membership)
    {
        await _context.Memberships.AddAsync(membership);
    }

    public Task UpdateAsync(Membership membership)
    {
        _context.Memberships.Update(membership);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Membership membership)
    {
        _context.Memberships.Remove(membership);
        return Task.CompletedTask;
    }
}