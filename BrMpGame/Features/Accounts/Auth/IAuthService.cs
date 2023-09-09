using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Features.Accounts.Auth;

public interface IAuthService
{
    Task<ActionResult<AuthResponse>> Login(AuthRequest request);
}