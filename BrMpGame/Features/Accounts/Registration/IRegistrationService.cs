using BrMpGame.Features.Accounts.Auth;

namespace BrMpGame.Features.Accounts.Registration;

public interface IRegistrationService
{
    Task<AuthResponse> Register(RegisterRequest request);
}