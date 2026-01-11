using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Database;

public class DatabaseContext : DbContext
{
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }


    public DatabaseContext() { }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("name=Database");

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(DatabaseContext))!);
}