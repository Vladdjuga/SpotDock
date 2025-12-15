using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Billing.Domain.Entities;

namespace SpotDock.Modules.Billing.Infrastructure.Persistence.Context;

public class BillingDbContext(DbContextOptions<BillingDbContext> options) : DbContext(options)
{
    public DbSet<Wallet> Wallets { get; init; }
    public DbSet<WalletTransaction> WalletTransactions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
