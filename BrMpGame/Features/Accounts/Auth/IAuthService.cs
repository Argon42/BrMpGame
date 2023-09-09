namespace BrMpGame.Features.Accounts.Auth;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
}