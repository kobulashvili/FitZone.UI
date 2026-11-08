using FitZone.Application.DTOs.Memberships;

namespace FitZone.Application.Interfaces.Services;

public interface IMembershipService
{
    Task<MembershipResponseDto> PurchaseAsync(PurchaseMembershipDto dto);

    Task<MembershipResponseDto> RenewAsync(int membershipId);

    Task<MembershipResponseDto?> GetByIdAsync(int membershipId);

    Task<List<MembershipResponseDto>> GetByMemberIdAsync(int memberId);
}