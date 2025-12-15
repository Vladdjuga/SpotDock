using SpotDock.Modules.Billing.Domain.Entities;

namespace SpotDock.Modules.Billing.Domain.Repositories;

public interface IWalletRepository
{
    Task<Wallet> CreateAsync(Wallet wallet);
    Task<Wallet> GetByUserIdAsync(Guid userId);
    Task<Wallet?> GetAsync(Guid walletId);
    Task<IList<Wallet>> GetAllAsync();
    Task UpdateAsync(Wallet wallet);
}
