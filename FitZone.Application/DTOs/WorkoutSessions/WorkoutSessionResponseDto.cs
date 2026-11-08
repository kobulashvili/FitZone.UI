using FitZone.Domain.Enums;

namespace FitZone.Application.DTOs.WorkoutSessions;

public class WorkoutSessionResponseDto
{
    public int Id { get; set; }
    public int TrainerId { get; set; }
    public WorkoutType WorkoutType { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int Capacity { get; set; }
    public int OccupiedPlaces { get; set; }
    public int AvailablePlaces { get; set; }
    public bool IsPremiumOnly { get; set; }
    public bool IsCancelled { get; set; }
}