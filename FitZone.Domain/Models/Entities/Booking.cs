using FitZone.Domain.Enums;
using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class Booking
{
    public int Id { get; private set; }

    public int MemberId { get; private set; }

    public Member Member { get; private set; } = null!;

    public int WorkoutSessionId { get; private set; }

    public WorkoutSession WorkoutSession { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public BookingStatus Status { get; private set; }

    private Booking()
    {
    }

    public Booking(
        int memberId,
        int workoutSessionId)
    {
        MemberId = memberId;
        WorkoutSessionId = workoutSessionId;

        CreatedAt = DateTime.UtcNow;
        Status = BookingStatus.Confirmed;
    }

    public void Confirm()
    {
        Status = BookingStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new InvalidOperationException(
                "Booking is already cancelled.");

        Status = BookingStatus.Cancelled;
    }
}