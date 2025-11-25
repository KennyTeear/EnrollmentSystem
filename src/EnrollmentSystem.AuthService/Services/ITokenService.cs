using EnrollmentSystem.AuthService.Models;

namespace EnrollmentSystem.AuthService.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    int? ValidateToken(string token);
}