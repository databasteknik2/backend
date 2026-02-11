using Backend.Infrastructure.Data;
using Backend.Presentation.API.Dtos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddOpenApi();
builder.Services.AddCors();

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    
app.MapOpenApi();
app.UseHttpsRedirection();

app.MapGet("/api/courses", async (AppDbContext db) =>
{
    var courses = await db.Courses
        .Select(c => new { c.Id, c.Title, c.Description })
        .ToListAsync();

    return Results.Ok(courses);
});

app.Run();

