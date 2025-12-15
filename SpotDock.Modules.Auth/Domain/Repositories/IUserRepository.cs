using SpotDock.Modules.Auth.Domain.Entities;

namespace SpotDock.Modules.Auth.Domain.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task DeleteAsync(Guid userId);
    Task<IList<User>> GetAll();
    Task<User> GetAsync(Guid userId);
    Task<User?> GetByEmailAsync(string email);
    Task UpdateAsync(User user);
}
