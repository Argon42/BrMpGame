using BrMpGame.Features.Accounts.Auth;
using BrMpGame.Features.Accounts.RefreshToken;
using BrMpGame.Features.Accounts.Registration;

namespace BrMpGame.Features.Accounts;

public static class AccountContainerBinding
{
    public static void AddAccount(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IRefreshTokenService, RefreshTokenService>();
        services.AddTransient<IRegistrationService, RegistrationService>();
    }
}