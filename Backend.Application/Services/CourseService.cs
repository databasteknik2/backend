using Backend.Application.Dtos;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace Backend.Application.Services;

public class CourseService
{
    private readonly IApplicationDbContext _db;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "all_courses"; 

    public CourseService(IApplicationDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<Course>> GetCoursesAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out List<Course>? courses))
        {
            courses = await _db.Courses.AsNoTracking().ToListAsync();
            _cache.Set(CacheKey, courses, TimeSpan.FromMinutes(10));
        }
        return courses!;
    }

    public async Task<Course> CreateCourseAsync(CourseDto dto)
    {
        var course = new Course(dto.Title, dto.Description);
        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        _cache.Remove(CacheKey); 
        return course;
    }

    public async Task<bool> UpdateCourseAsync(int id, CourseDto dto)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return false;

        course.Update(dto.Title, dto.Description);
        await _db.SaveChangesAsync();

        _cache.Remove(CacheKey); 
        return true;
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return false;

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();

        _cache.Remove(CacheKey); 
        return true;
    }
}
