using BrMpGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BrMpGame;

internal static class ApplicationConfiguration
{
    private const string DefaultAdminUserName = "Admin";

    private const string DefaultAdminPassword = "Admin123!";

    internal static async Task CreateDefaultRolesAndUsers(IServiceProvider services)
    {
        using IServiceScope scope = services.CreateScope();
        DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();

        if (await context.Database.EnsureCreatedAsync() == false)
        {
            await context.Database.MigrateAsync();
        }

        CreateDefaultRoles(context, scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
        CreateDefaultUsers(context, scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>());
    }

    private static void CreateDefaultUsers(DataContext dataContext, UserManager<AppUser> userManager)
    {
        if (dataContext.Users.Any()) return;

        AppUser user = new()
        {
            UserName = DefaultAdminUserName,
        };
        IdentityResult result = userManager.CreateAsync(user, DefaultAdminPassword).Result;
        if (result.Succeeded) 
            userManager.AddToRoleAsync(user, Roles.Admin).Wait();
    }

    private static void CreateDefaultRoles(DataContext dataContext, RoleManager<IdentityRole> roleManager)
    {
        if (dataContext.Roles.Any())
            return;
        foreach (string role in Roles.All)
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
    }
}