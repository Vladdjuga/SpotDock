using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Auctions.Domain.Entities;

namespace SpotDock.Modules.Auctions.Infrastructure.Persistence.Context;

public class AuctionsDbContext(DbContextOptions<AuctionsDbContext> options) : DbContext(options)
{
    public DbSet<Bid> Bids { get; init; }
    public DbSet<SpotInstance> SpotInstances { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}