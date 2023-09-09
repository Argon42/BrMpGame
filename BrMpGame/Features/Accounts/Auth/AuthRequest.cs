namespace BrMpGame.Features.Accounts.Auth;

public class AuthRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}