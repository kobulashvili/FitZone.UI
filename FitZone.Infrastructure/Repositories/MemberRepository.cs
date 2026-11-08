using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;
using FitZone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitZone.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly FitZoneDbContext _context;

    public MemberRepository(FitZoneDbContext context)
    {
        _context = context;
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Member?> GetByEmailAsync(string email)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task<List<Member>> GetAllAsync()
    {
        return await _context.Members
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Member member)
    {
        await _context.Members.AddAsync(member);
    }

    public Task UpdateAsync(Member member)
    {
        _context.Members.Update(member);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Member member)
    {
        _context.Members.Remove(member);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Members
            .AnyAsync(m => m.Email == email);
    }
}