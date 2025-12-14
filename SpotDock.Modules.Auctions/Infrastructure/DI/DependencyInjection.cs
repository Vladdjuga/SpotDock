using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotDock.Modules.Auctions.Domain.Repositories;
using SpotDock.Modules.Auctions.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Auctions.Infrastructure.DI;

public static class DependencyInjection
{
    public static void AddAuctionsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuctionsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("auctionsDb"))
        );

        services.AddScoped<IBidRepository>(); // TODO : Add implementation
        services.AddScoped<ISpotInstanceRepository>(); // TODO : Add implementation

        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}