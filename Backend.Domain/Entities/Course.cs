namespace Backend.Domain.Entities;

public class Course
{
    public int Id { get; private set; }

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    private readonly List<CourseEvent> _events = new();
    public IReadOnlyCollection<CourseEvent> Events => _events;

    private Course() { }

    public Course(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
