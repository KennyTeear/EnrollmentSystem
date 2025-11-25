using System;

namespace EnrollmentSystem.Shared.Models;

public class GradeDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public decimal? NumericGrade { get; set; }
    public string? LetterGrade { get; set; }
    public string Semester { get; set; } = string.Empty;
    public DateTime? DatePosted { get; set; }
}