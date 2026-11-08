using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Interfaces;
public interface IVisitRepository
{
    Task<Visit?> GetByIdAsync(int id);

    Task<List<Visit>> GetByMemberIdAsync(int memberId);

    Task AddAsync(Visit visit);

    Task<List<Visit>> GetAllAsync();
}