namespace Backend.Domain.Entities;

public class CourseEvent
{
    public int Id { get; private set; }

    public int CourseId { get; private set; }
    public Course Course { get; private set; } = null!;

    public int TeacherId { get; private set; }
    public Teacher Teacher { get; private set; } = null!;

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public string Location { get; private set; } = null!;
    public int Capacity { get; private set; }

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments;

    private CourseEvent() { }

    public CourseEvent(
        int courseId,
        int teacherId,
        DateTime startDate,
        DateTime endDate,
        string location,
        int capacity)
    {
        CourseId = courseId;
        TeacherId = teacherId;
        StartDate = startDate;
        EndDate = endDate;
        Location = location;
        Capacity = capacity;
    }
}
