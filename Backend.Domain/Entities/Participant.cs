namespace Backend.Domain.Entities;

public class Participant
{
    public int Id { get; private set; }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments;

    private Participant() { }

    public Participant(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public string FullName => $"{FirstName} {LastName}";
}
