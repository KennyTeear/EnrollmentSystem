using EnrollmentSystem.GradeService.Data;
using EnrollmentSystem.GradeService.Models;
using EnrollmentSystem.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnrollmentSystem.GradeService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly GradeDbContext _context;

    public GradesController(GradeDbContext context)
    {
        _context = context;
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetStudentGrades(int studentId)
    {
        var grades = await _context.Grades
            .Where(g => g.StudentId == studentId)
            .OrderByDescending(g => g.DatePosted)
            .ToListAsync();

        var gradeDtos = grades.Select(g => new GradeDto
        {
            Id = g.Id,
            StudentId = g.StudentId,
            CourseId = g.CourseId,
            CourseName = g.CourseName,
            CourseCode = g.CourseCode,
            NumericGrade = g.NumericGrade,
            LetterGrade = g.LetterGrade,
            Semester = g.Semester,
            DatePosted = g.DatePosted
        });

        return Ok(gradeDtos);
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Faculty")]
    public async Task<IActionResult> UploadGrade([FromBody] UploadGradeRequest request)
    {
        try
        {
            // Set default course info (you can improve this by calling Course Service)
            var courseName = $"Course {request.CourseId}";
            var courseCode = $"COURSE{request.CourseId}";

            // Check if grade already exists
            var existingGrade = await _context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == request.StudentId && g.CourseId == request.CourseId);

            if (existingGrade != null)
            {
                // Update existing grade
                existingGrade.NumericGrade = request.NumericGrade;
                existingGrade.LetterGrade = request.LetterGrade;
                existingGrade.DatePosted = DateTime.UtcNow;
            }
            else
            {
                // Create new grade
                var grade = new Grade
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    CourseName = courseName,
                    CourseCode = courseCode,
                    NumericGrade = request.NumericGrade,
                    LetterGrade = request.LetterGrade,
                    Semester = "Fall 2024"
                };

                _context.Grades.Add(grade);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Grade uploaded successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to upload grade", error = ex.Message });
        }
    }

    [HttpGet("course/{courseId}")]
    [Authorize(Roles = "Faculty")]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetCourseGrades(int courseId)
    {
        var grades = await _context.Grades
            .Where(g => g.CourseId == courseId)
            .ToListAsync();

        var gradeDtos = grades.Select(g => new GradeDto
        {
            Id = g.Id,
            StudentId = g.StudentId,
            CourseId = g.CourseId,
            CourseName = g.CourseName,
            CourseCode = g.CourseCode,
            NumericGrade = g.NumericGrade,
            LetterGrade = g.LetterGrade,
            Semester = g.Semester,
            DatePosted = g.DatePosted
        });

        return Ok(gradeDtos);
    }
}