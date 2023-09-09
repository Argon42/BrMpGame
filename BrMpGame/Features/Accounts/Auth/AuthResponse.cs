namespace BrMpGame.Features.Accounts.Auth;

public class AuthResponse
{
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}