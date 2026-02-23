using Backend.Application.Dtos;
using Backend.Application.Services;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Backend.Tests;

public class CourseServiceIntegrationTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var db = new AppDbContext(options);
        db.Database.OpenConnection(); 
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task CreateCourseAsync_ShouldSaveToDatabase()
    {
        // Arrange
        var db = GetDbContext();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new CourseService(db, cache);
        var dto = new CourseDto("Integrationstest", "Beskrivning");

        // Act
        var result = await service.CreateCourseAsync(dto);

        // Assert
        var courseInDb = await db.Courses.FindAsync(result.Id);
        Assert.NotNull(courseInDb);
        Assert.Equal("Integrationstest", courseInDb.Title);
    }
}