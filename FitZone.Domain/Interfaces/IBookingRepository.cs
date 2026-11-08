using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int id);

    Task<List<Booking>> GetByMemberIdAsync(int memberId);

    Task<List<Booking>> GetBySessionIdAsync(int sessionId);

    Task<bool> HasActiveBookingAsync(int memberId, int sessionId);

    Task AddAsync(Booking booking);

    Task UpdateAsync(Booking booking);

    Task DeleteAsync(Booking booking);
}