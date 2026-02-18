using Backend.Application.Dtos;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services;

public class CourseEventService
{
    private readonly IApplicationDbContext _db;

    public CourseEventService(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<CourseEvent>> GetCourseEventsAsync()
    {
        return await _db.CourseEvents
            .Include(e => e.Course)
            .Include(e => e.Teacher)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<CourseEvent>> GetCourseEventsByCourseIdAsync(int courseId)
    {
        return await _db.CourseEvents
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Teacher)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CourseEvent> CreateCourseEventAsync(int courseId, int teacherId, DateTime startDate, DateTime endDate, string location, int capacity)
    {
        var courseEvent = new CourseEvent(courseId, teacherId, startDate, endDate, location, capacity);
        _db.CourseEvents.Add(courseEvent);
        await _db.SaveChangesAsync();
        return courseEvent;
    }

    public async Task<bool> UpdateCourseEventAsync(int id, DateTime startDate, DateTime endDate, string location, int capacity)
    {
        var courseEvent = await _db.CourseEvents.FindAsync(id);
        if (courseEvent == null) return false;

        courseEvent.Update(startDate, endDate, location, capacity); // uppdatera direkt

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCourseEventAsync(int id)
    {
        var courseEvent = await _db.CourseEvents.FindAsync(id);
        if (courseEvent == null) return false;

        _db.CourseEvents.Remove(courseEvent);
        await _db.SaveChangesAsync();
        return true;
    }
}
