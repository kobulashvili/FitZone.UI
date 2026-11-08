using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Interfaces;

public interface ITrainerRepository
{
    Task<Trainer?> GetByIdAsync(int id);

    Task<List<Trainer>> GetAllAsync();

    Task AddAsync(Trainer trainer);

    Task UpdateAsync(Trainer trainer);

    Task DeleteAsync(Trainer trainer);
}