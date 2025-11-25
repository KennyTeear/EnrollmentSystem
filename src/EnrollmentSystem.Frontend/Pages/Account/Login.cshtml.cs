using EnrollmentSystem.Frontend.Services;
using EnrollmentSystem.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnrollmentSystem.Frontend.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IAuthServiceClient _authService;

    public LoginModel(IAuthServiceClient authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = new LoginRequest
        {
            Username = Input.Username,
            Password = Input.Password
        };

        var response = await _authService.LoginAsync(request);

        if (!response.Success)
        {
            ErrorMessage = response.Message;
            return Page();
        }

        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, response.User!.Id.ToString()),
            new Claim(ClaimTypes.Name, response.User.Username),
            new Claim(ClaimTypes.Email, response.User.Email),
            new Claim(ClaimTypes.Role, response.User.Role),
            new Claim("AccessToken", response.Token)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToPage("/Index");
    }
}