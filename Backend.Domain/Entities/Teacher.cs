namespace Backend.Domain.Entities;

public class Teacher
{
    public int Id { get; private set; }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    private readonly List<CourseEvent> _courseEvents = new();
    public IReadOnlyCollection<CourseEvent> CourseEvents => _courseEvents;

    private Teacher() { }

    public Teacher(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public string FullName => $"{FirstName} {LastName}";
}
