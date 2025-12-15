using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Auth.Domain.Entities;

namespace SpotDock.Modules.Auth.Infrastructure.Persistence.Context;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
