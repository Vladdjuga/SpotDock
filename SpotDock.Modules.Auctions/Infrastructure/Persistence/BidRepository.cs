using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Auctions.Domain.Entities;
using SpotDock.Modules.Auctions.Domain.Repositories;
using SpotDock.Modules.Auctions.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Auctions.Infrastructure.Persistence;

public sealed class BidRepository(AuctionsDbContext context) : IBidRepository
{
    public async Task<Bid> CreateAsync(Bid bid)
    {
        context.Bids.Add(bid);
        await context.SaveChangesAsync();
        return bid;
    }

    public async Task DeleteAsync(Guid bidId)
    {
        var entity = await context.Bids.FirstOrDefaultAsync(b => b.Id == bidId);
        if (entity is null)
        {
            return;
        }

        context.Bids.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IList<Bid>> GetAll()
    {
        return await context.Bids
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Bid> GetAsync(Guid bidId)
    {
        return await context.Bids
            .AsNoTracking()
            .FirstAsync(b => b.Id == bidId);
    }
}
