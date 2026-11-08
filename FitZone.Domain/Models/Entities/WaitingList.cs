using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class WaitingList
{
    public int Id { get; private set; }

    public int MemberId { get; private set; }

    public Member Member { get; private set; } = null!;

    public int WorkoutSessionId { get; private set; }

    public WorkoutSession WorkoutSession { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public bool IsProcessed { get; private set; }

    private WaitingList()
    {
    }

    public WaitingList(int memberId, int workoutSessionId)
    {
        MemberId = memberId;
        WorkoutSessionId = workoutSessionId;
        CreatedAt = DateTime.UtcNow;
        IsProcessed = false;
    }

    public void MarkAsProcessed()
    {
        IsProcessed = true;
    }
}