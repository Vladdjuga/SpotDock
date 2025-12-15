using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Market.Domain.Entities;

namespace SpotDock.Modules.Market.Infrastructure.Persistence.Context;

public class AuctionsDbContext(DbContextOptions<AuctionsDbContext> options) : DbContext(options)
{
    public DbSet<SpotInstance> SpotInstances { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}