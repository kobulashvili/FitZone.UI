using FitZone.Domain.Enums;

namespace FitZone.Domain.Models.Entities;

public abstract class Membership
{
    public int Id { get; private set; }

    public int MemberId { get; private set; }

    public Member Member { get; private set; } = null!;

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public int VisitLimit { get; private set; }

    public int RemainingVisits { get; private set; }

    public MembershipStatus Status { get; private set; }

    public abstract MembershipType Type { get; }

    protected Membership()
    {
    }

    protected Membership(
     int memberId,
     int visitLimit,
     int durationDays)
    {
        MemberId = memberId;

        VisitLimit = visitLimit;
        RemainingVisits = visitLimit;

        StartDate = DateTime.UtcNow;
        EndDate = StartDate.AddDays(durationDays);

        Status = MembershipStatus.Active;
    }

    public abstract decimal GetPrice();

    public abstract bool CanBook(WorkoutSession session);

    public bool CanEnter()
    {
        return Status == MembershipStatus.Active
            && DateTime.UtcNow <= EndDate
            && RemainingVisits > 0;
    }

    public void RegisterVisit()
    {
        if (!CanEnter())
            throw new InvalidOperationException(
                "Membership is expired or has no remaining visits.");

        RemainingVisits--;

        if (RemainingVisits == 0)
            Expire();
    }

    public void Expire()
    {
        Status = MembershipStatus.Expired;
    }

    public void Renew(int visitLimit, int durationDays)
    {
        if (visitLimit <= 0 || durationDays <= 0)
            throw new ArgumentException("Invalid membership parameters.");

        StartDate = DateTime.UtcNow;
        EndDate = StartDate.AddDays(durationDays);

        VisitLimit = visitLimit;
        RemainingVisits = visitLimit;

        Status = MembershipStatus.Active;
    }
}