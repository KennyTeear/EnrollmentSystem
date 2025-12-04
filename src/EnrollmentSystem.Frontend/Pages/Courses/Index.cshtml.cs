using EnrollmentSystem.Frontend.Services;
using EnrollmentSystem.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnrollmentSystem.Frontend.Pages.Courses;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ICourseServiceClient _courseService;

    public IndexModel(ICourseServiceClient courseService)
    {
        _courseService = courseService;
    }

    public IEnumerable<CourseDto>? Courses { get; set; }
    public bool ServiceUnavailable { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var token = User.FindFirst("AccessToken")?.Value;
            //await Response.WriteAsync("<script>console.log('Hello World');</script>");
            if (string.IsNullOrEmpty(token))
            {
                ServiceUnavailable = true;
                return;
            }

            Courses = await _courseService.GetCoursesAsync(token);
            ServiceUnavailable = Courses == null;
        }
        catch
        {
            ServiceUnavailable = true;
        }
            
    }

    public async Task<IActionResult> OnPostEnrollAsync(int courseId)
    {
        try
        {
            var token = User.FindFirst("AccessToken")?.Value;
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(studentIdClaim))
            {
                return RedirectToPage();
            }

            var request = new EnrollmentRequest
            {
                StudentId = int.Parse(studentIdClaim),
                CourseId = courseId
            };

            await _courseService.EnrollInCourseAsync(request, token);
            return RedirectToPage();
        }
        catch
        {
            ServiceUnavailable = true;

            return RedirectToPage();
        }
            
    }
}