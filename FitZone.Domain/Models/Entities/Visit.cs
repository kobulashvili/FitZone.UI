using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class Visit
{
    public int Id { get; private set; }

    public int MemberId { get; private set; }

    public Member Member { get; private set; } = null!;

    public int MembershipId { get; private set; }

    public Membership Membership { get; private set; } = null!;

    public DateTime VisitDate { get; private set; }

    private Visit()
    {
    }

    public Visit(int memberId, int membershipId)
    {
        MemberId = memberId;
        MembershipId = membershipId;
        VisitDate = DateTime.UtcNow;
    }
}