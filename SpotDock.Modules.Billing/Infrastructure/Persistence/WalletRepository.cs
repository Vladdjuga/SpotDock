using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Billing.Domain.Entities;
using SpotDock.Modules.Billing.Domain.Repositories;
using SpotDock.Modules.Billing.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Billing.Infrastructure.Persistence;

public sealed class WalletRepository(BillingDbContext context) : IWalletRepository
{
    public async Task<Wallet> CreateAsync(Wallet wallet)
    {
        context.Wallets.Add(wallet);
        await context.SaveChangesAsync();
        return wallet;
    }

    public async Task<Wallet> GetByUserIdAsync(Guid userId)
    {
        return await context.Wallets
            .AsNoTracking()
            .FirstAsync(w => w.UserId == userId);
    }

    public async Task<Wallet?> GetAsync(Guid walletId)
    {
        return await context.Wallets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == walletId);
    }

    public async Task<IList<Wallet>> GetAllAsync()
    {
        return await context.Wallets
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        context.Wallets.Update(wallet);
        await context.SaveChangesAsync();
    }
}
