namespace FitZone.Application.DTOs.Visits;

public class VisitResponseDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int MembershipId { get; set; }
    public DateTime VisitDate { get; set; }
}