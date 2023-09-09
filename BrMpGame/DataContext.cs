using BrMpGame.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrMpGame;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
}