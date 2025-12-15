using SpotDock.Modules.Billing.Domain.Entities;

namespace SpotDock.Modules.Billing.Domain.Repositories;

public interface IWalletTransactionRepository
{
    Task<WalletTransaction> CreateAsync(WalletTransaction transaction);
    Task<IList<WalletTransaction>> GetByWalletIdAsync(Guid walletId);
    Task<WalletTransaction?> GetAsync(Guid transactionId);
}
