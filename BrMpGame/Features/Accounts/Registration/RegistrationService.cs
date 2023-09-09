using BrMpGame.Features.Accounts.Auth;
using BrMpGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrMpGame.Features.Accounts.Registration;

public class RegistrationService : IRegistrationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthService _authService;
    private readonly DataContext _context;

    public RegistrationService(DataContext context, UserManager<AppUser> userManager, IAuthService authService)
    {
        _context = context;
        _userManager = userManager;
        _authService = authService;
    }
    
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        AppUser user = new() { UserName = request.UserName };
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new BadRequestObjectResult(new { Request = request, result.Errors });

        AppUser? findUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);

        if (findUser == null)
            throw new Exception($"User {request.UserName} not found");

        await _userManager.AddToRoleAsync(findUser, Roles.User);

        return await _authService.Login(new AuthRequest
        {
            UserName = request.UserName,
            Password = request.Password,
        });
    }
}