using SpotDock.Modules.Auctions.Domain.Entities;

namespace SpotDock.Modules.Auctions.Domain.Repositories;

public interface IBidRepository
{
    Task<Bid> CreateAsync(Bid bid);
    Task DeleteAsync(Guid bidId);
    Task<IList<Bid>> GetAll();
    Task<Bid> GetAsync(Guid bidId);
}