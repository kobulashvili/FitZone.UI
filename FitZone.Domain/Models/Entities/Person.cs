namespace FitZone.Domain.Models.Entities;

public abstract class Person
{
    public int Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    protected Person()
    {
    }

    protected Person(string fullName, string email, string passwordHash)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
    }

    protected void UpdatePersonalInfo(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.");

        FullName = fullName.Trim();
    }
}