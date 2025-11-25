using EnrollmentSystem.Shared.Models;
using System.Threading.Tasks;

namespace EnrollmentSystem.Frontend.Services;

public interface IAuthServiceClient
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> LogoutAsync(string token);
    Task<bool> ValidateTokenAsync(string token);
}