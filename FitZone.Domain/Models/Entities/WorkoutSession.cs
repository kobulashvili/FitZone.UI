using FitZone.Domain.Enums;
using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class WorkoutSession
{
    public int Id { get; private set; }

    public int TrainerId { get; private set; }

    public Trainer Trainer { get; private set; } = null!;

    public WorkoutType WorkoutType { get; private set; }

    public DateTime StartTime { get; private set; }

    public int DurationMinutes { get; private set; }

    public int Capacity { get; private set; }

    public bool IsPremiumOnly { get; private set; }

    public bool IsCancelled { get; private set; }

    public ICollection<Booking> Bookings { get; private set; }
        = new List<Booking>();

    public int OccupiedPlaces =>
        Bookings.Count(b => b.Status == BookingStatus.Confirmed);

    public int AvailablePlaces => Capacity - OccupiedPlaces;

    private WorkoutSession()
    {
    }

    public WorkoutSession(
        int trainerId,
        WorkoutType workoutType,
        DateTime startTime,
        int durationMinutes,
        int capacity,
        bool isPremiumOnly)
    {
        if (startTime <= DateTime.UtcNow)
            throw new ArgumentException("Session must be in the future.");

        if (durationMinutes <= 0)
            throw new ArgumentException("Invalid duration.");

        if (capacity <= 0)
            throw new ArgumentException("Invalid capacity.");

        TrainerId = trainerId;
        WorkoutType = workoutType;
        StartTime = startTime;
        DurationMinutes = durationMinutes;
        Capacity = capacity;
        IsPremiumOnly = isPremiumOnly;
    }

    public void AddMember(Booking booking)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Session is cancelled.");

        if (AvailablePlaces <= 0)
            throw new InvalidOperationException("Session is full.");

        if (Bookings.Any(b =>
            b.MemberId == booking.MemberId &&
            b.Status == BookingStatus.Confirmed))
        {
            throw new InvalidOperationException(
                "Member is already booked.");
        }

        booking.Confirm();

        Bookings.Add(booking);
    }

    public void UpdateDetails(
    int trainerId,
    WorkoutType workoutType,
    DateTime startTime,
    int durationMinutes,
    int capacity,
    bool isPremiumOnly)
    {
        if (IsCancelled)
            throw new InvalidOperationException(
                "Cannot update a cancelled session.");

        if (StartTime <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "Cannot update a session that has already started.");

        if (startTime <= DateTime.UtcNow)
            throw new ArgumentException(
                "Session must be in the future.");

        if (durationMinutes <= 0)
            throw new ArgumentException("Invalid duration.");

        if (capacity <= 0)
            throw new ArgumentException("Invalid capacity.");

        if (capacity < OccupiedPlaces)
            throw new InvalidOperationException(
                "Capacity cannot be less than occupied places.");

        TrainerId = trainerId;
        WorkoutType = workoutType;
        StartTime = startTime;
        DurationMinutes = durationMinutes;
        Capacity = capacity;
        IsPremiumOnly = isPremiumOnly;
    }
    public void RemoveMember(Booking booking)
    {
        if (!Bookings.Contains(booking))
            throw new InvalidOperationException("Booking not found.");

        booking.Cancel();
    }

    public void CancelSession()
    {
        IsCancelled = true;

        foreach (var booking in Bookings.Where(
            b => b.Status == BookingStatus.Confirmed))
        {
            booking.Cancel();
        }
    }
}