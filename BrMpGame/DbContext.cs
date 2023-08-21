using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BrMpGame;

public class ApplicationContext : DbContext
{
    public DbSet<IdentityUser> Users { get; set; } = default!;
    
    public ApplicationContext(IConfiguration configuration)
    {
        Database.EnsureCreated();
    }
}