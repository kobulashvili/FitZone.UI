using FitZone.Application.DTOs.Members;

namespace FitZone.Application.Interfaces.Services;

public interface IMemberService
{
    Task<MemberResponseDto> RegisterAsync(RegisterMemberDto dto);

    Task<MemberResponseDto?> LoginAsync(LoginMemberDto dto);
}