using FitZone.Domain.Enums;

namespace FitZone.Application.DTOs.Memberships;

public class PurchaseMembershipDto
{
    public int MemberId { get; set; }
    public MembershipType Type { get; set; }
}