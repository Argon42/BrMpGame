using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BrMpGame.Extensions;
using BrMpGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Features.Accounts.RefreshToken;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public RefreshTokenService(UserManager<AppUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<TokenModel> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
            throw new ArgumentNullException(nameof(tokenModel));

        string? accessToken = tokenModel.AccessToken;
        string? refreshToken = tokenModel.RefreshToken;
        ClaimsPrincipal? principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
            throw new Exception("Invalid access token or refresh token");

        string? username = principal.Identity!.Name;
        AppUser? user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new Exception("Invalid access token or refresh token");

        JwtSecurityToken newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
        string newRefreshToken = _configuration.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new TokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken,
        };
    }

    public async Task Revoke(string username)
    {
        AppUser? user = await _userManager.FindByNameAsync(username);
        if (user == null)
            throw new Exception($"User {username} not found");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
    }

    public async Task RevokeAll()
    {
        List<AppUser> users = _userManager.Users.ToList();
        foreach (AppUser user in users)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }
    }
}