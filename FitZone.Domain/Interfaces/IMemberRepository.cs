using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id);

    Task<Member?> GetByEmailAsync(string email);

    Task<List<Member>> GetAllAsync();

    Task AddAsync(Member member);

    Task UpdateAsync(Member member);

    Task DeleteAsync(Member member);

    Task<bool> ExistsByEmailAsync(string email);
}