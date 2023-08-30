using System.IdentityModel.Tokens.Jwt;
using BrMpGame.Extensions;
using BrMpGame.Models;
using Microsoft.AspNetCore.Identity;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(AppUser user, List<IdentityRole> roles)
    {
        JwtSecurityToken token = user
            .CreateClaims(roles)
            .CreateJwtToken(_configuration);
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}