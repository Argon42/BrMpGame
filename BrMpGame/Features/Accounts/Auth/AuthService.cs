using System.Security.Authentication;
using BrMpGame.Extensions;
using BrMpGame.Models;
using BrMpGame.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrMpGame.Features.Accounts.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthService(ITokenService tokenService, DataContext context, UserManager<AppUser> userManager,
        IConfiguration configuration)
    {
        _tokenService = tokenService;
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        AppUser? managedUser = await _userManager.FindByNameAsync(request.UserName);

        if (managedUser == null)
            throw new AuthenticationException("Invalid username or password");

        bool isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);

        if (!isPasswordValid)
            throw new AuthenticationException("Invalid username or password");

        AppUser user = await _context.Users.FirstAsync(u => u.UserName == request.UserName);

        List<string> roleIds =
            await _context.UserRoles.Where(r => r.UserId == user.Id).Select(x => x.RoleId).ToListAsync();
        List<IdentityRole> roles = _context.Roles.Where(x => roleIds.Contains(x.Id)).ToList();

        string accessToken = _tokenService.CreateToken(user, roles);
        user.RefreshToken = _configuration.GenerateRefreshToken();
        user.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(_configuration.GetSection("JwtSettings:RefreshTokenValidityInDays").Get<int>());

        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Username = user.UserName!,
            Token = accessToken,
            RefreshToken = user.RefreshToken,
        };
    }
}