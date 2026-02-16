using Backend.Domain.Entities;
using Xunit;

namespace Backend.Tests;

public class CourseTests
{
    [Fact]
    public void Update_ShouldChangeTitleAndDescription()
    {
        // Arrange 
        var course = new Course("Gammal titel", "Gammal beskrivning");

        // Act 
        course.Update("Ny titel", "Ny beskrivning");

        // Assert 
        Assert.Equal("Ny titel", course.Title);
        Assert.Equal("Ny beskrivning", course.Description);
    }
}