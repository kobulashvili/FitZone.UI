using FitZone.Domain.Models.Entities;

namespace FitZone.Domain.Models.Entities;

public class Member : Person
{
    public ICollection<Membership> Memberships { get; private set; }
        = new List<Membership>();

    public ICollection<Booking> Bookings { get; private set; }
        = new List<Booking>();

    public ICollection<Visit> Visits { get; private set; }
        = new List<Visit>();

    protected Member()
    {
    }

    public Member(
        string fullName,
        string email,
        string passwordHash)
        : base(fullName, email, passwordHash)
    {
    }
}