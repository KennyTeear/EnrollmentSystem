namespace EnrollmentSystem.Shared.Models;

public class UploadGradeRequest
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal NumericGrade { get; set; }
    public string LetterGrade { get; set; } = string.Empty;
}