using EnrollmentSystem.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnrollmentSystem.Frontend.Services;

public interface ICourseServiceClient
{
    Task<IEnumerable<CourseDto>?> GetCoursesAsync(string token);
    Task<CourseDto?> GetCourseAsync(int id, string token);
    Task<bool> EnrollInCourseAsync(EnrollmentRequest request, string token);
    Task<IEnumerable<CourseDto>?> GetStudentCoursesAsync(int studentId, string token);
}   