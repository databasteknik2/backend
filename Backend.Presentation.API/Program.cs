using Backend.Application.Dtos;
using Backend.Application.Interfaces;
using Backend.Application.Services;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseEventService>();


var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Courses.Any())
    {
        var teacher = new Teacher("Anna", "Andersson", "anna@utbildning.se");
        var course = new Course(".NET 10 Masterclass", "Lär dig framtidens backend.");
        db.Teachers.Add(teacher);
        db.Courses.Add(course);
        db.SaveChanges();

        var courseEvent = new CourseEvent(course.Id, teacher.Id, DateTime.Now.AddDays(7), DateTime.Now.AddDays(12), "Solna", 20);
        db.CourseEvents.Add(courseEvent);
        db.SaveChanges();
    }
}

app.MapGet("/api/courses", async (CourseService service) =>
{
    var courses = await service.GetCoursesAsync();
    return Results.Ok(courses);
});

app.MapPost("/api/courses", async (CourseDto dto, CourseService service) =>
{
    var course = await service.CreateCourseAsync(dto);
    return Results.Created($"/api/courses/{course.Id}", course);
});

app.MapPut("/api/courses/{id}", async (int id, CourseDto dto, CourseService service) =>
{
    var updated = await service.UpdateCourseAsync(id, dto);
    if (!updated) return Results.NotFound();
    return Results.NoContent();
});

app.MapDelete("/api/courses/{id}", async (int id, CourseService service) =>
{
    var deleted = await service.DeleteCourseAsync(id);
    if (!deleted) return Results.NotFound();
    return Results.Ok($"Kursen {id} borttagen.");
});


app.MapGet("/api/courses/{courseId}/events", async (int courseId, CourseEventService service) =>
{
    var events = await service.GetCourseEventsByCourseIdAsync(courseId);
    return Results.Ok(events);
});


app.MapGet("/api/courseevents", async (CourseEventService service) =>
{
    var events = await service.GetCourseEventsAsync();
    return Results.Ok(events);
});

app.MapPost("/api/courseevents", async (int courseId, int teacherId, DateTime startDate, DateTime endDate, string location, int capacity, CourseEventService service) =>
{
    var courseEvent = await service.CreateCourseEventAsync(courseId, teacherId, startDate, endDate, location, capacity);
    return Results.Created($"/api/courseevents/{courseEvent.Id}", courseEvent);
});

app.MapPut("/api/courseevents/{id}", async (int id, DateTime startDate, DateTime endDate, string location, int capacity, CourseEventService service) =>
{
    var updated = await service.UpdateCourseEventAsync(id, startDate, endDate, location, capacity);
    if (!updated) return Results.NotFound();
    return Results.NoContent();
});

app.MapDelete("/api/courseevents/{id}", async (int id, CourseEventService service) =>
{
    var deleted = await service.DeleteCourseEventAsync(id);
    if (!deleted) return Results.NotFound();
    return Results.Ok($"Kurstillfälle {id} borttaget.");
});





app.MapPost("/api/enroll", async (EnrollRequest request, IApplicationDbContext db) =>
{
    using var transaction = await db.BeginTransactionAsync();
    try
    {
        var courseEvent = await db.CourseEvents.FindAsync(request.CourseEventId);
        if (courseEvent == null) return Results.NotFound("Kurstillfället hittades inte.");

        var participant = await db.Participants.FirstOrDefaultAsync(p => p.Email == request.Email);
        if (participant == null)
        {
            participant = new Participant(request.FirstName, request.LastName, request.Email);
            db.Participants.Add(participant);
            await db.SaveChangesAsync();
        }

        var enrollment = new Enrollment(request.CourseEventId, participant.Id);
        db.Enrollments.Add(enrollment);

        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        return Results.Ok($"Registrerad på {courseEvent.Location}!");
    }
    catch (Exception)
    {
        await transaction.RollbackAsync();
        return Results.BadRequest("Något gick fel vid anmälningen.");
    }
});


app.MapGet("/api/teachers", async (IApplicationDbContext db) =>
{
    return Results.Ok(await db.Teachers.AsNoTracking().ToListAsync());
});

app.MapPost("/api/teachers", async (TeacherDto dto, IApplicationDbContext db) =>
{
    var teacher = new Teacher(dto.FirstName, dto.LastName, dto.Email);
    db.Teachers.Add(teacher);
    await db.SaveChangesAsync();
    return Results.Created($"/api/teachers/{teacher.Id}", teacher);
});


app.MapGet("/api/participants", async (IApplicationDbContext db) =>
{
    return Results.Ok(await db.Participants.AsNoTracking().ToListAsync());
});

app.MapGet("/api/stats/gmail", async (AppDbContext db) =>
{
    var count = await db.Participants
        .FromSqlRaw("SELECT * FROM Participants WHERE Email LIKE '%@gmail.com'")
        .CountAsync();

    return Results.Ok(new { GmailUsers = count });
});

app.Run();