using Backend.Application.Dtos;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class TeacherService
{
    private readonly IApplicationDbContext _db;

    public TeacherService(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Teacher>> GetTeachersAsync()
    {
        return await _db.Teachers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Teacher> CreateTeacherAsync(TeacherDto dto)
    {
        var teacher = new Teacher(dto.FirstName, dto.LastName, dto.Email);

        _db.Teachers.Add(teacher);
        await _db.SaveChangesAsync();

        return teacher;
    }
}