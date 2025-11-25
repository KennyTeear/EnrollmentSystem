using EnrollmentSystem.CourseService.Data;
using EnrollmentSystem.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollmentSystem.CourseService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly CourseDbContext _context;

    public CoursesController(CourseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var courses = await _context.Courses.ToListAsync();
        
        var courseDtos = courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Description = c.Description,
            MaxStudents = c.MaxStudents,
            EnrolledStudents = _context.Enrollments.Count(e => e.CourseId == c.Id),
            IsOpen = c.IsOpen,
            Instructor = c.Instructor
        });

        return Ok(courseDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        var courseDto = new CourseDto
        {
            Id = course.Id,
            Code = course.Code,
            Name = course.Name,
            Description = course.Description,
            MaxStudents = course.MaxStudents,
            EnrolledStudents = await _context.Enrollments.CountAsync(e => e.CourseId == course.Id),
            IsOpen = course.IsOpen,
            Instructor = course.Instructor
        };

        return Ok(courseDto);
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> EnrollInCourse([FromBody] EnrollmentRequest request)
    {
        var course = await _context.Courses.FindAsync(request.CourseId);
        if (course == null)
        {
            return NotFound(new { message = "Course not found" });
        }

        if (!course.IsOpen)
        {
            return BadRequest(new { message = "Course is not open for enrollment" });
        }

        var enrolledCount = await _context.Enrollments.CountAsync(e => e.CourseId == request.CourseId);
        if (enrolledCount >= course.MaxStudents)
        {
            return BadRequest(new { message = "Course is full" });
        }

        var existingEnrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == request.StudentId && e.CourseId == request.CourseId);

        if (existingEnrollment != null)
        {
            return BadRequest(new { message = "Already enrolled in this course" });
        }

        var enrollment = new Models.Enrollment
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Successfully enrolled in course" });
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetStudentCourses(int studentId)
    {
        var enrolledCourseIds = await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Select(e => e.CourseId)
            .ToListAsync();

        var courses = await _context.Courses
            .Where(c => enrolledCourseIds.Contains(c.Id))
            .ToListAsync();

        var courseDtos = courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Description = c.Description,
            MaxStudents = c.MaxStudents,
            EnrolledStudents = _context.Enrollments.Count(e => e.CourseId == c.Id),
            IsOpen = c.IsOpen,
            Instructor = c.Instructor
        });

        return Ok(courseDtos);
    }
}