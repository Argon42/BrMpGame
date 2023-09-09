using BrMpGame.Features.Accounts.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Features.Accounts.Registration;

public interface IRegistrationService
{
    Task<ActionResult<AuthResponse>> Register(RegisterRequest request);
}