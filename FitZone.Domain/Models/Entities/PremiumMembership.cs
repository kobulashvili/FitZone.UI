using FitZone.Domain.Enums;
using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class PremiumMembership : Membership
{
    public override MembershipType Type => MembershipType.Premium;

    protected PremiumMembership()
    {
    }

    public PremiumMembership(int memberId)
        : base(memberId, 20, 30)
    {
    }

    public override decimal GetPrice() => 90m;

    public override bool CanBook(WorkoutSession session)
    {
        return CanEnter();
    }
}