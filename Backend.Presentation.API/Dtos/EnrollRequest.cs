namespace Backend.Presentation.API.Dtos;

public record EnrollRequest(string FirstName, string LastName, string Email, int CourseEventId);
