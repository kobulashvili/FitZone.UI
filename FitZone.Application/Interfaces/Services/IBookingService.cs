using FitZone.Application.DTOs.Bookings;

namespace FitZone.Application.Interfaces.Services;

public interface IBookingService
{
    Task<BookingResponseDto> BookAsync(CreateBookingDto dto);

    Task CancelAsync(int bookingId);

    Task<List<BookingResponseDto>> GetByMemberIdAsync(int memberId);

    Task<List<BookingResponseDto>> GetBySessionIdAsync(int sessionId);
}