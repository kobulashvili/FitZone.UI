namespace FitZone.Application.DTOs.Trainers;

public class CreateTrainerDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
}