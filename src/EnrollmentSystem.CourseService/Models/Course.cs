namespace EnrollmentSystem.CourseService.Models;

public class Course
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
    public bool IsOpen { get; set; }
    public string Instructor { get; set; } = string.Empty;
}