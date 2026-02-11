using Backend.Domain.Entities;
using Xunit;

namespace Backend.Tests;

public class CourseTests
{
    [Fact]
    public void Update_ShouldChangeTitleAndDescription()
    {
        // Arrange - Skapa en kurs
        var course = new Course("Gammal titel", "Gammal beskrivning");

        // Act - Kör metoden du vill testa
        course.Update("Ny titel", "Ny beskrivning");

        // Assert - Kontrollera att det blev rätt
        Assert.Equal("Ny titel", course.Title);
        Assert.Equal("Ny beskrivning", course.Description);
    }
}