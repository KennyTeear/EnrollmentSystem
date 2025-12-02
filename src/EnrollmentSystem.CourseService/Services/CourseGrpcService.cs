using Grpc.Core;
using EnrollmentSystem.CourseService.Grpc;
using EnrollmentSystem.CourseService.Data;
using EnrollmentSystem.CourseService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollmentSystem.CourseService.Services;

public class CourseGrpcService : CourseGrpc.CourseGrpcBase
{
    private readonly CourseDbContext _db;

    public CourseGrpcService(CourseDbContext db)
    {
        _db = db;
    }

    public override async Task<CourseListReply> GetAllCourses(Empty request, ServerCallContext context)
    {
        var courses = await _db.Courses.ToListAsync();
        var reply = new CourseListReply();
        reply.Courses.AddRange(courses.Select(c => new CourseReply
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Description = c.Description,
            MaxStudents = c.MaxStudents,
            IsOpen = c.IsOpen,
            Instructor = c.Instructor
        }));
        return reply;
    }

    public override async Task<CourseReply> GetCourseById(CourseByIdRequest request, ServerCallContext context)
    {
        var course = await _db.Courses.FindAsync(request.CourseId);
        if (course == null) return new CourseReply();
        return new CourseReply
        {
            Id = course.Id,
            Code = course.Code,
            Name = course.Name,
            Description = course.Description,
            MaxStudents = course.MaxStudents,
            IsOpen = course.IsOpen,
            Instructor = course.Instructor
        };
    }

    public override async Task<EnrollReply> EnrollInCourse(EnrollRequest request, ServerCallContext context)
    {
        var course = await _db.Courses.FindAsync(request.CourseId);
        if (course == null || !course.IsOpen)
            return new EnrollReply { Success = false, Error = "Course not found or not open" };

        var alreadyEnrolled = await _db.Enrollments.AnyAsync(e => e.StudentId == request.StudentId && e.CourseId == request.CourseId);
        if (alreadyEnrolled)
            return new EnrollReply { Success = false, Error = "Already enrolled" };

        var enrollment = new Enrollment { StudentId = request.StudentId, CourseId = request.CourseId, EnrolledAt = DateTime.UtcNow };
        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();
        return new EnrollReply { Success = true };
    }

    public override async Task<CourseListReply> GetStudentCourses(StudentCoursesRequest request, ServerCallContext context)
    {
        var enrollments = await _db.Enrollments
            .Where(e => e.StudentId == request.StudentId)
            .Include(e => e.Course)
            .ToListAsync();

        var reply = new CourseListReply();
        reply.Courses.AddRange(enrollments.Select(e =>
        {
            var course = e.Course;
            return new CourseReply
            {
                Id = course?.Id ?? 0,
                Code = course?.Code ?? string.Empty,
                Name = course?.Name ?? string.Empty,
                Description = course?.Description ?? string.Empty,
                MaxStudents = course?.MaxStudents ?? 0,
                IsOpen = course?.IsOpen ?? false,
                Instructor = course?.Instructor ?? string.Empty
            };
        }));
        return reply;
    }
}