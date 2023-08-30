using BrMpGame;
using BrMpGame.Models;
using Microsoft.AspNetCore.Identity;

internal static class ApplicationConfiguration
{
    private const string DefaultAdminUserName = "Admin";
    private const string DefaultAdminPassword = "Admin123!";

    internal static void CreateDefaultRolesAndUsers(IServiceProvider services)
    {
        using IServiceScope scope = services.CreateScope();
        DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();

        context.Database.EnsureCreated();

        CreateDefaultRoles(context, scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
        CreateDefaultUsers(context, scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>());
    }

    private static void CreateDefaultUsers(DataContext dataContext, UserManager<AppUser> userManager1)
    {
        if (dataContext.Users.Any()) return;

        AppUser user = new()
        {
            UserName = DefaultAdminUserName,
        };
        IdentityResult result = userManager1.CreateAsync(user, DefaultAdminPassword).Result;
        if (result.Succeeded)
        {
            userManager1.AddToRoleAsync(user, Roles.Admin).Wait();
        }
    }

    private static void CreateDefaultRoles(DataContext dataContext, RoleManager<IdentityRole> roleManager1)
    {
        if (dataContext.Roles.Any())
            return;
        foreach (string role in Roles.All)
            roleManager1.CreateAsync(new IdentityRole(role)).Wait();
    }
}