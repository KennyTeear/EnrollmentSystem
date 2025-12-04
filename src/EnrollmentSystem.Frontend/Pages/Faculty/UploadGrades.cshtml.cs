using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EnrollmentSystem.Frontend.Services;
using EnrollmentSystem.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EnrollmentSystem.Frontend.Pages.Faculty;

[Authorize(Roles = "Faculty")]
public class UploadGradesModel : PageModel
{
    private readonly ICourseServiceClient _courseService;
    private readonly IGradeServiceClient _gradeService;

    public UploadGradesModel(ICourseServiceClient courseService, IGradeServiceClient gradeService)
    {
        _courseService = courseService;
        _gradeService = gradeService;
    }

    public List<CourseDto> Courses { get; set; } = new();

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? Message { get; set; }

    public bool ServiceAvailable { get; set; }

    public class InputModel
    {
        [Required]
        public int CourseId { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        public decimal NumericGrade { get; set; }
        
        [Required]
        public string LetterGrade { get; set; } = string.Empty;
    }

    public async Task OnGetAsync()
    {
        try
        {
            var token = User.FindFirst("AccessToken")?.Value ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                var courses = await _courseService.GetCoursesAsync(token);
                Courses = courses?.ToList() ?? new List<CourseDto>();
                ServiceAvailable = true;
            }
            else
            {
                ServiceAvailable = false;
            }
        }
        catch
        {
            Message = "This service is currently unavailable. Please try again later.";
            ServiceAvailable = false;
        }
            
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var token = User.FindFirst("AccessToken")?.Value ?? "";

            var request = new UploadGradeRequest
            {
                StudentId = Input.StudentId,
                CourseId = Input.CourseId,
                NumericGrade = Input.NumericGrade,
                LetterGrade = Input.LetterGrade
            };

            var success = await _gradeService.UploadGradeAsync(request, token);

            if (success)
            {
                Message = "Grade uploaded successfully!";
                ModelState.Clear();
                Input = new InputModel();
            }
            else
            {
                Message = "Failed to upload grade. Please try again.";
            }

            await OnGetAsync();
            return Page();
        }
        catch
        {
            Message = "Something went wrong. Try again later.";
            ServiceAvailable = false;
            return Page();
        }
    }
}