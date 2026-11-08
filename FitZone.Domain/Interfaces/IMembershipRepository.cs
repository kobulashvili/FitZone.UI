using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Interfaces;

public interface IMembershipRepository
{
    Task<Membership?> GetByIdAsync(int id);

    Task<List<Membership>> GetByMemberIdAsync(int memberId);

    Task AddAsync(Membership membership);

    Task UpdateAsync(Membership membership);

    Task DeleteAsync(Membership membership);
}