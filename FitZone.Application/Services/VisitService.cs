
using AutoMapper;
using FitZone.Application.DTOs.Visits;
using FitZone.Application.Interfaces.Services;
using FitZone.Domain.Enums;
using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;

namespace FitZone.Application.Services;

public class VisitService : IVisitService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VisitService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VisitResponseDto> RegisterVisitAsync(
        RegisterVisitDto dto)
    {
        var member = await _unitOfWork.Members
            .GetByIdAsync(dto.MemberId);

        if (member is null)
        {
            throw new InvalidOperationException("Member not found.");
        }

        var membership = await _unitOfWork.Memberships
            .GetByIdAsync(dto.MembershipId);

        if (membership is null)
        {
            throw new InvalidOperationException("Membership not found.");
        }

        if (membership.MemberId != dto.MemberId)
        {
            throw new InvalidOperationException(
                "This membership does not belong to this member.");
        }

        if (membership.Status != MembershipStatus.Active)
        {
            throw new InvalidOperationException(
                "Membership is not active.");
        }

        if (membership.EndDate < DateTime.Now)
        {
            membership.Expire();

            await _unitOfWork.Memberships.UpdateAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            throw new InvalidOperationException(
                "Membership has expired.");
        }

        if (membership.RemainingVisits <= 0)
        {
            throw new InvalidOperationException(
                "No visits remaining.");
        }

        membership.RegisterVisit();

        var visit = new Visit(
            dto.MemberId,
            dto.MembershipId
        );

        await _unitOfWork.Visits.AddAsync(visit);

        await _unitOfWork.Memberships.UpdateAsync(membership);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<VisitResponseDto>(visit);
    }

    public async Task<List<VisitResponseDto>> GetByMemberIdAsync(
        int memberId)
    {
        var visits = await _unitOfWork.Visits
            .GetByMemberIdAsync(memberId);

        return _mapper.Map<List<VisitResponseDto>>(visits);
    }

    public async Task<List<VisitResponseDto>> GetAllAsync()
    {
        var visits = await _unitOfWork.Visits.GetAllAsync();

        return _mapper.Map<List<VisitResponseDto>>(visits);
    }
}
