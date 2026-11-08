using FitZone.Domain.Enums;

namespace FitZone.Application.DTOs.Bookings;

public class BookingResponseDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int WorkoutSessionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public BookingStatus Status { get; set; }
}