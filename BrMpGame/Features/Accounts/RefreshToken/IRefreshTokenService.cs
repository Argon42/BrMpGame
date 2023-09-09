using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Features.Accounts.RefreshToken;

public interface IRefreshTokenService
{
    Task<ActionResult<TokenModel>> RefreshToken(TokenModel? tokenModel);
    Task<IActionResult> Revoke(string username);
    Task<IActionResult> RevokeAll();
}