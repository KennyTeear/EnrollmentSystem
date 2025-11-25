using EnrollmentSystem.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnrollmentSystem.Frontend.Services;

public interface IGradeServiceClient
{
    Task<IEnumerable<GradeDto>?> GetStudentGradesAsync(int studentId, string token);
    Task<bool> UploadGradeAsync(UploadGradeRequest request, string token);
}