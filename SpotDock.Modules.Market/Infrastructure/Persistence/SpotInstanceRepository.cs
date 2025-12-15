using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Market.Domain.Entities;
using SpotDock.Modules.Market.Domain.Repositories;
using SpotDock.Modules.Market.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Market.Infrastructure.Persistence;

public sealed class SpotInstanceRepository(AuctionsDbContext context) : ISpotInstanceRepository
{
    public async Task<SpotInstance> CreateAsync(SpotInstance spot)
    {
        context.SpotInstances.Add(spot);
        await context.SaveChangesAsync();
        return spot;
    }

    public async Task DeleteAsync(Guid spotId)
    {
        var entity = await context.SpotInstances.FirstOrDefaultAsync(s => s.Id == spotId);
        if (entity is null)
        {
            return;
        }

        context.SpotInstances.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IList<SpotInstance>> GetAll()
    {
        return await context.SpotInstances
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<SpotInstance> GetAsync(Guid spotId)
    {
        return await context.SpotInstances
            .AsNoTracking()
            .FirstAsync(s => s.Id == spotId);
    }
}
