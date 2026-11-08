namespace FitZone.Domain.Interfaces;

public interface IUnitOfWork
{
    IMemberRepository Members { get; }
    ITrainerRepository Trainers { get; }
    IMembershipRepository Memberships { get; }
    IWorkoutSessionRepository WorkoutSessions { get; }
    IBookingRepository Bookings { get; }
    IVisitRepository Visits { get; }

    Task<int> SaveChangesAsync();
}