using EnrollmentSystem.Frontend.Services;
using EnrollmentSystem.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnrollmentSystem.Frontend.Pages.Grades;

[Authorize(Roles = "Student")]
public class IndexModel : PageModel
{
    private readonly IGradeServiceClient _gradeService;

    public IndexModel(IGradeServiceClient gradeService)
    {
        _gradeService = gradeService;
    }

    public IEnumerable<GradeDto>? Grades { get; set; }
    public bool ServiceUnavailable { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var token = User.FindFirst("AccessToken")?.Value;
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(studentIdClaim))
            {
                ServiceUnavailable = true;
                return;
            }

            var studentId = int.Parse(studentIdClaim);
            Grades = await _gradeService.GetStudentGradesAsync(studentId, token);
            ServiceUnavailable = Grades == null;
        }
        catch
        {
            Response.Redirect("/Index", true);
            return;
        }
            
    }
}