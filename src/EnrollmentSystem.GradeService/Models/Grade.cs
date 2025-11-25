using System;

namespace EnrollmentSystem.GradeService.Models;

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public decimal NumericGrade { get; set; }
    public string LetterGrade { get; set; } = string.Empty;
    public string Semester { get; set; } = string.Empty;
    public DateTime DatePosted { get; set; } = DateTime.UtcNow;
}