using FitZone.Domain.Enums;
using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;
using FitZone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitZone.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly FitZoneDbContext _context;

    public BookingRepository(FitZoneDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Booking>> GetByMemberIdAsync(int memberId)
    {
        return await _context.Bookings
            .Where(b => b.MemberId == memberId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Booking>> GetBySessionIdAsync(int sessionId)
    {
        return await _context.Bookings
            .Where(b => b.WorkoutSessionId == sessionId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> HasActiveBookingAsync(int memberId, int sessionId)
    {
        return await _context.Bookings
            .AnyAsync(b =>
                b.MemberId == memberId &&
                b.WorkoutSessionId == sessionId &&
                b.Status == BookingStatus.Confirmed);
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Booking booking)
    {
        _context.Bookings.Remove(booking);
        return Task.CompletedTask;
    }
}