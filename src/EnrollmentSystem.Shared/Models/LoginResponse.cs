namespace EnrollmentSystem.Shared.Models;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
}