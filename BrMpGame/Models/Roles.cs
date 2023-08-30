namespace BrMpGame.Models;

public static class Roles
{
    public const string Admin = nameof(Admin);
    public const string User = nameof(User);
    
    public static IReadOnlyList<string> All => new[] { Admin, User };
}