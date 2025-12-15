using SpotDock.Modules.Market.Domain.Entities;

namespace SpotDock.Modules.Market.Domain.Repositories;

public interface ISpotInstanceRepository
{
    Task<SpotInstance> CreateAsync(SpotInstance spot);
    Task DeleteAsync(Guid spotId);
    Task<IList<SpotInstance>> GetAll();
    Task<SpotInstance> GetAsync(Guid spotId);
}