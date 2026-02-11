using Backend.Domain.Entities;
using Backend.Application.Interfaces; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Course> Courses { get; }
    DbSet<CourseEvent> CourseEvents { get; }
    DbSet<Teacher> Teachers { get; }
    DbSet<Participant> Participants { get; }
    DbSet<Enrollment> Enrollments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}