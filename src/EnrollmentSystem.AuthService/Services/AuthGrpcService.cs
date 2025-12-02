using Grpc.Core;
using EnrollmentSystem.AuthService.Grpc; // Namespace from your proto file
using EnrollmentSystem.AuthService.Services; // Your authentication logic
using EnrollmentSystem.Shared.Models;

public class AuthGrpcService : AuthGrpc.AuthGrpcBase
{
    private readonly IAuthenticationService _authService;

    public AuthGrpcService(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public override async Task<LoginReply> Login(EnrollmentSystem.AuthService.Grpc.LoginRequest request, ServerCallContext context)
    {
        var loginRequest = new EnrollmentSystem.Shared.Models.LoginRequest
        {
            Username = request.Username,
            Password = request.Password
        };

        var response = await _authService.AuthenticateAsync(loginRequest);

        if (!response.Success)
        {
            return new LoginReply { Error = response.ErrorMessage ?? "Invalid credentials" };
        }

        return new LoginReply { Token = response.Token };
    }

}