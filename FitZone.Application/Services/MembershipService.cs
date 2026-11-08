
using AutoMapper;
using FitZone.Application.DTOs.Memberships;
using FitZone.Application.Interfaces.Services;
using FitZone.Domain.Enums;
using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;

namespace FitZone.Application.Services;

public class MembershipService : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MembershipService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MembershipResponseDto> PurchaseAsync(
        PurchaseMembershipDto dto)
    {
        var member = await _unitOfWork.Members
            .GetByIdAsync(dto.MemberId);

        if (member is null)
        {
            throw new InvalidOperationException("Member not found.");
        }

        var memberships = await _unitOfWork.Memberships
            .GetByMemberIdAsync(dto.MemberId);

        bool hasActiveMembership = memberships.Any(m =>
            m.Status == MembershipStatus.Active &&
            m.EndDate >= DateTime.Now &&
            m.RemainingVisits > 0);

        if (hasActiveMembership)
        {
            throw new InvalidOperationException(
                "Member already has an active membership.");
        }

        Membership membership = dto.Type switch
        {
            MembershipType.Basic => new BasicMembership(dto.MemberId),
            MembershipType.Premium => new PremiumMembership(dto.MemberId),
            _ => throw new ArgumentException("Invalid membership type.")
        };

        await _unitOfWork.Memberships.AddAsync(membership);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MembershipResponseDto>(membership);
    }

    public async Task<MembershipResponseDto> RenewAsync(int membershipId)
    {
        var membership = await _unitOfWork.Memberships
            .GetByIdAsync(membershipId);

        if (membership is null)
        {
            throw new InvalidOperationException("Membership not found.");
        }

        int visitLimit = membership.Type switch
        {
            MembershipType.Basic => 8,
            MembershipType.Premium => 20,
            _ => throw new InvalidOperationException(
                "Invalid membership type.")
        };

        membership.Renew(visitLimit, 30);

        await _unitOfWork.Memberships.UpdateAsync(membership);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MembershipResponseDto>(membership);
    }

    public async Task<MembershipResponseDto?> GetByIdAsync(
        int membershipId)
    {
        var membership = await _unitOfWork.Memberships
            .GetByIdAsync(membershipId);

        if (membership is null)
        {
            return null;
        }

        return _mapper.Map<MembershipResponseDto>(membership);
    }

    public async Task<List<MembershipResponseDto>> GetByMemberIdAsync(
        int memberId)
    {
        var memberships = await _unitOfWork.Memberships
            .GetByMemberIdAsync(memberId);

        return _mapper.Map<List<MembershipResponseDto>>(memberships);
    }
}
