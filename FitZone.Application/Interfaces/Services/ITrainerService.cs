using FitZone.Application.DTOs.Trainers;

namespace FitZone.Application.Interfaces.Services;

public interface ITrainerService
{
    Task<TrainerResponseDto> CreateAsync(CreateTrainerDto dto);

    Task<TrainerResponseDto?> GetByIdAsync(int trainerId);

    Task<List<TrainerResponseDto>> GetAllAsync();

    Task UpdateAsync(int trainerId, UpdateTrainerDto dto);

    Task DeleteAsync(int trainerId);
}