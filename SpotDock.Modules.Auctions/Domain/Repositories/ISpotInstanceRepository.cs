using SpotDock.Modules.Auctions.Domain.Entities;

namespace SpotDock.Modules.Auctions.Domain.Repositories;

public interface ISpotInstanceRepository
{
    Task<SpotInstance> CreateAsync(SpotInstance spot);
    Task DeleteAsync(Guid spotId);
    Task<IList<SpotInstance>> GetAll();
    Task<SpotInstance> GetAsync(Guid spotId);
}