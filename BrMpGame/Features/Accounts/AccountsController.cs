using BrMpGame.Features.Accounts.Auth;
using BrMpGame.Features.Accounts.RefreshToken;
using BrMpGame.Features.Accounts.Registration;
using BrMpGame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Features.Accounts;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request,
        [FromServices] IAuthService authService)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await authService.Login(request);
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<ActionResult<TokenModel>> RefreshToken(TokenModel? tokenModel,
        [FromServices] IRefreshTokenService refreshTokenService)
    {
        return await refreshTokenService.RefreshToken(tokenModel);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterRequest request,
        [FromServices] IRegistrationService registrationService)
    {
        if (!ModelState.IsValid)
            return BadRequest(request);

        return await registrationService.Register(request);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username, [FromServices] IRefreshTokenService refreshTokenService)
    {
        return await refreshTokenService.Revoke(username);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll([FromServices] IRefreshTokenService refreshTokenService)
    {
        return await refreshTokenService.RevokeAll();
    }
}