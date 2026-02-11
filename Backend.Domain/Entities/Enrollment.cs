namespace Backend.Domain.Entities;

public class Enrollment
{
    public int Id { get; private set; }

    public int CourseEventId { get; private set; }
    public CourseEvent CourseEvent { get; private set; } = null!;

    public int ParticipantId { get; private set; }
    public Participant Participant { get; private set; } = null!;

    public DateTime RegisteredAt { get; private set; }
    public EnrollmentStatus Status { get; private set; }

    private Enrollment() { }

    public Enrollment(int courseEventId, int participantId)
    {
        CourseEventId = courseEventId;
        ParticipantId = participantId;
        RegisteredAt = DateTime.UtcNow;
        Status = EnrollmentStatus.Registered;
    }

    public void Cancel()
    {
        Status = EnrollmentStatus.Cancelled;
    }
}
