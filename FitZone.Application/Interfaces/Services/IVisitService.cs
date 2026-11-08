using FitZone.Application.DTOs.Visits;

namespace FitZone.Application.Interfaces.Services;

public interface IVisitService
{
    Task<VisitResponseDto> RegisterVisitAsync(RegisterVisitDto dto);

    Task<List<VisitResponseDto>> GetByMemberIdAsync(int memberId);

    Task<List<VisitResponseDto>> GetAllAsync();
}