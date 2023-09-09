using Microsoft.AspNetCore.Identity;

namespace BrMpGame.Models;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}