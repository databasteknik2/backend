using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.MapGet("/api/courses", async (IApplicationDbContext db) =>
{
    var courses = await db.Courses
        .AsNoTracking()
        .Select(c => new { c.Id, c.Title, c.Description })
        .ToListAsync();

    return Results.Ok(courses);
});

app.MapPost("/api/enroll", async (string firstName, string lastName, string email, int courseEventId, IApplicationDbContext db) =>
{
    using var transaction = await db.BeginTransactionAsync();
    try
    {
        var participant = new Participant(firstName, lastName, email);
        db.Participants.Add(participant);
        await db.SaveChangesAsync();

        var enrollment = new Enrollment(courseEventId, participant.Id);
        db.Enrollments.Add(enrollment);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();
        return Results.Created($"/api/enrollments/{enrollment.Id}", enrollment);
    }
    catch (Exception)
    {
        await transaction.RollbackAsync();
        return Results.BadRequest("Kunde inte genomföra registreringen.");
    }
});

app.MapGet("/api/stats/gmail-users", async (AppDbContext db) =>
{
    var gmailUsers = await db.Participants
        .FromSqlRaw("SELECT * FROM Participants WHERE Email LIKE '%@gmail.com%'")
        .ToListAsync();

    return Results.Ok(new
    {
        count = gmailUsers.Count,
        message = "Hämtat via rå SQL-fråga"
    });
});

app.MapPut("/api/participants/{id}", async (int id, string newEmail, IApplicationDbContext db) =>
{
    var participant = await db.Participants.FindAsync(id);
    if (participant is null) return Results.NotFound();

    typeof(Participant).GetProperty("Email")?.SetValue(participant, newEmail);

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/participants/{id}", async (int id, IApplicationDbContext db) =>
{
    var participant = await db.Participants.FindAsync(id);
    if (participant is null) return Results.NotFound();

    var enrollments = db.Enrollments.Where(e => e.ParticipantId == id);
    db.Enrollments.RemoveRange(enrollments);

    db.Participants.Remove(participant);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();