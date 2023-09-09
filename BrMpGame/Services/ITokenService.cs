using BrMpGame.Models;
using Microsoft.AspNetCore.Identity;

namespace BrMpGame.Services;

public interface ITokenService
{
    string CreateToken(AppUser appUser, List<IdentityRole> roles);
}