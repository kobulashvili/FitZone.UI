using FitZone.Domain.Enums;

namespace FitZone.Application.DTOs.WorkoutSessions;

public class CreateWorkoutSessionDto
{
    public int TrainerId { get; set; }
    public WorkoutType WorkoutType { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int Capacity { get; set; }
    public bool IsPremiumOnly { get; set; }
}