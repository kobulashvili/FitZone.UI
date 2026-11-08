using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class Manager : Person
{
    protected Manager()
    {
    }

    public Manager(
        string fullName,
        string email,
        string passwordHash)
        : base(fullName, email, passwordHash)
    {
    }
}