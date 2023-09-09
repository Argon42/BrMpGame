using System.Security.Authentication;
using BrMpGame.Features.Accounts.Auth;
using BrMpGame.Features.Accounts.RefreshToken;
using BrMpGame.Features.Accounts.Registration;
using BrMpGame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Features.Accounts;

[ApiController]
[Route("api/v1/accounts")]
public class AccountsController : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request,
        [FromServices] IAuthService authService)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            AuthResponse authResponse = await authService.Login(request);
            return Ok(authResponse);
        }
        catch (AuthenticationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<ActionResult<TokenModel>> RefreshToken(TokenModel tokenModel,
        [FromServices] IRefreshTokenService refreshTokenService)
    {
        try
        {
            TokenModel refreshToken = await refreshTokenService.RefreshToken(tokenModel);
            return Ok(refreshToken);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterRequest request,
        [FromServices] IRegistrationService registrationService)
    {
        if (!ModelState.IsValid)
            return BadRequest(request);

        try
        {
            AuthResponse authResponse = await registrationService.Register(request);
            return Ok(authResponse);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username, [FromServices] IRefreshTokenService refreshTokenService)
    {
        try
        {
            await refreshTokenService.Revoke(username);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll([FromServices] IRefreshTokenService refreshTokenService)
    {
        try
        {
            await refreshTokenService.RevokeAll();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}