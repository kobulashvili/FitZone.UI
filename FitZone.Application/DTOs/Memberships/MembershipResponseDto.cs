using FitZone.Domain.Enums;

namespace FitZone.Application.DTOs.Memberships;

public class MembershipResponseDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public MembershipType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int VisitLimit { get; set; }
    public int RemainingVisits { get; set; }
    public MembershipStatus Status { get; set; }
    public decimal Price { get; set; }
}