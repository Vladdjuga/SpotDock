using Microsoft.EntityFrameworkCore;
using SpotDock.Modules.Auth.Domain.Entities;
using SpotDock.Modules.Auth.Domain.Repositories;
using SpotDock.Modules.Auth.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Auth.Infrastructure.Persistence;

public sealed class UserRepository(AuthDbContext context) : IUserRepository
{
    public async Task<User> CreateAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid userId)
    {
        var entity = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (entity is null)
        {
            return;
        }

        context.Users.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IList<User>> GetAll()
    {
        return await context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User> GetAsync(Guid userId)
    {
        return await context.Users
            .AsNoTracking()
            .FirstAsync(u => u.Id == userId);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}
