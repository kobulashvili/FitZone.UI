using FitZone.Domain.Interfaces;
using FitZone.Infrastructure.Data;
using FitZone.Infrastructure.Repositories;

namespace FitZone.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly FitZoneDbContext _context;

    public IMemberRepository Members { get; }
    public ITrainerRepository Trainers { get; }
    public IMembershipRepository Memberships { get; }
    public IWorkoutSessionRepository WorkoutSessions { get; }
    public IBookingRepository Bookings { get; }
    public IVisitRepository Visits { get; }

    public UnitOfWork(FitZoneDbContext context)
    {
        _context = context;

        Members = new MemberRepository(_context);
        Trainers = new TrainerRepository(_context);
        Memberships = new MembershipRepository(_context);
        WorkoutSessions = new WorkoutSessionRepository(_context);
        Bookings = new BookingRepository(_context);
        Visits = new VisitRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}