using FitZone.Domain.Enums;

namespace FitZone.Domain.Models.Entities;

public class BasicMembership : Membership
{
    public override MembershipType Type => MembershipType.Basic;

    protected BasicMembership()
    {
    }

    public BasicMembership(int memberId)
        : base(memberId, 8, 30)
    {
    }

    public override decimal GetPrice()
    {
        return 50m;
    }

    public override bool CanBook(WorkoutSession session)
    {
        return CanEnter() && !session.IsPremiumOnly;
    }
}