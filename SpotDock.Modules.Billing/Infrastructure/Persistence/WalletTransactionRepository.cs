using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Billing.Domain.Entities;
using SpotDock.Modules.Billing.Domain.Repositories;
using SpotDock.Modules.Billing.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Billing.Infrastructure.Persistence;

public sealed class WalletTransactionRepository(BillingDbContext context) : IWalletTransactionRepository
{
    public async Task<WalletTransaction> CreateAsync(WalletTransaction transaction)
    {
        context.WalletTransactions.Add(transaction);
        await context.SaveChangesAsync();
        return transaction;
    }

    public async Task<IList<WalletTransaction>> GetByWalletIdAsync(Guid walletId)
    {
        return await context.WalletTransactions
            .AsNoTracking()
            .Where(t => t.WalletId == walletId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<WalletTransaction?> GetAsync(Guid transactionId)
    {
        return await context.WalletTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == transactionId);
    }
}
