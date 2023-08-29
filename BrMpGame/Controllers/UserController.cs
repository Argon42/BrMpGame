using BrMpGame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrMpGame.Controllers;

[Authorize] // Добавляем авторизацию ко всем методам контроллера
public class UserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private DataContext _context;

    public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, DataContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
    }

    [AllowAnonymous] // Отключаем авторизацию для этого метода
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { Message = "Registration successful" });
            }

            return BadRequest(new { Message = "Registration failed", result.Errors });
        }
        return BadRequest(ModelState);
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult PublicData()
    {
        return Ok(new { Message = "This is public data" });
    }

    [HttpGet("user")]
    [Authorize(Roles = Roles.User)] // Требуем роль "User" для доступа
    public IActionResult UserData()
    {
        return Ok(new { Message = "This is user data" });
    }

    [HttpGet("admin")]
    [Authorize(Roles = Roles.Admin)] // Требуем роль "Admin" для доступа
    public IActionResult AdminData()
    {
        return Ok(new { Message = "This is admin data" });
    }
}


public class RegistrationModel
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}