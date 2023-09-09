using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Features.Accounts.RefreshToken;

public interface IRefreshTokenService
{
    Task<TokenModel> RefreshToken(TokenModel tokenModel);
    Task Revoke(string username);
    Task RevokeAll();
}