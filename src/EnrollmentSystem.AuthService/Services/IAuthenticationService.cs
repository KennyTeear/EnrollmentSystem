using EnrollmentSystem.Shared.Models;
using System.Threading.Tasks;

namespace EnrollmentSystem.AuthService.Services;

public interface IAuthenticationService
{
    Task<LoginResponse> AuthenticateAsync(LoginRequest request);
    Task<bool> LogoutAsync(int userId);
}