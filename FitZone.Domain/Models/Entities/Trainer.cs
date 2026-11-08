using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class Trainer : Person
{
    public string Specialization { get; private set; } = string.Empty;

    public ICollection<WorkoutSession> WorkoutSessions { get; private set; }
        = new List<WorkoutSession>();

    protected Trainer()
    {
    }

    public Trainer(
        string fullName,
        string email,
        string passwordHash,
        string specialization)
        : base(fullName, email, passwordHash)
    {
        Specialization = specialization;
    }

    public void UpdateDetails(string fullName, string specialization)
    {
        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Specialization is required.");

        UpdatePersonalInfo(fullName);

        Specialization = specialization.Trim();
    }
}