using BrMpGame.Models;
using Microsoft.AspNetCore.Identity;

public interface ITokenService
{
    string CreateToken(AppUser appUser, List<IdentityRole> roles);
}