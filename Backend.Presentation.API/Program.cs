using Backend.Application.Interfaces;
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

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

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

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.MapGet("/api/courses", async (IApplicationDbContext db, IMemoryCache cache) =>
{
    const string cacheKey = "course_list";
    if (!cache.TryGetValue(cacheKey, out var courses))
    {
        courses = await db.Courses.AsNoTracking().ToListAsync();
        cache.Set(cacheKey, courses, TimeSpan.FromMinutes(5));
    }
    return Results.Ok(courses);
});


app.MapPost("/api/enroll", async (int courseEventId, string email, string firstName, string lastName, IApplicationDbContext db) =>
{
    using var transaction = await db.BeginTransactionAsync();
    try
    {
        var courseEvent = await db.CourseEvents.FindAsync(courseEventId);
        if (courseEvent == null)
        {
            return Results.NotFound("Kurstillfället hittades inte.");
        }

        var participant = await db.Participants.FirstOrDefaultAsync(p => p.Email == email);
        if (participant == null)
        {
            participant = new Participant(firstName, lastName, email);
            db.Participants.Add(participant);
            await db.SaveChangesAsync();
        }

        var enrollment = new Enrollment(courseEventId, participant.Id);
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


app.MapGet("/api/stats/gmail", async (IApplicationDbContext db) =>
{
    
    var count = await db.Participants
        .FromSqlRaw("SELECT * FROM Participants WHERE Email LIKE '%@gmail.com%'")
        .CountAsync();

    return Results.Ok(new { GmailUsers = count });
});

app.MapPut("/api/courses/{id}", async (int id, string title, string description, IApplicationDbContext db) =>
{
    var course = await db.Courses.FindAsync(id);
    if (course is null) return Results.NotFound();

    course.Update(title, description);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/courses/{id}", async (int id, IApplicationDbContext db) =>
{
    var course = await db.Courses.FindAsync(id);
    if (course is null) return Results.NotFound();

    db.Courses.Remove(course);
    await db.SaveChangesAsync();
    return Results.Ok($"Kursen {id} borttagen.");
});

app.Run();