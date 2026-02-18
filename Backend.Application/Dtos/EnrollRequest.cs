namespace Backend.Application.Dtos;

public record EnrollRequest(string FirstName, string LastName, string Email, int CourseEventId);
