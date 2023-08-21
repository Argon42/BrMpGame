using BrMpGame.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<User> Users { get; set; }
    
    public ApplicationContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration["DefaultConnection"]);
    }
}