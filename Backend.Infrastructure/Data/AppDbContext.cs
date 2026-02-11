using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseEvent> CourseEvents => Set<CourseEvent>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CourseEvent>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CourseEvent>()
            .HasOne(e => e.Teacher)
            .WithMany(t => t.CourseEvents)
            .HasForeignKey(e => e.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Participant)
            .WithMany(p => p.Enrollments)
            .HasForeignKey(e => e.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.CourseEvent)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseEventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
