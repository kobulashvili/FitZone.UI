
using FitZone.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitZone.Infrastructure.Data;

public class FitZoneDbContext : DbContext
{
    public FitZoneDbContext(DbContextOptions<FitZoneDbContext> options)
        : base(options)
    {
    }

    // Users
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Manager> Managers => Set<Manager>();

    // Memberships
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<BasicMembership> BasicMemberships => Set<BasicMembership>();
    public DbSet<PremiumMembership> PremiumMemberships => Set<PremiumMembership>();

    // Workout
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();

    // Bookings and visits
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Visit> Visits => Set<Visit>();

    // Waiting list
    public DbSet<WaitingList> WaitingLists => Set<WaitingList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Membership)
            .WithMany()
            .HasForeignKey(v => v.MembershipId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
