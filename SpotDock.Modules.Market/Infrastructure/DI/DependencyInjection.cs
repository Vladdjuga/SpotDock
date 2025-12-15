using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotDock.Modules.Market.Domain.Repositories;
using SpotDock.Modules.Market.Infrastructure.Persistence;
using SpotDock.Modules.Market.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Market.Infrastructure.DI;

public static class DependencyInjection
{
    public static void AddMarketModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuctionsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("auctionsDb"))
        );
        
        services.AddScoped<ISpotInstanceRepository, SpotInstanceRepository>();

        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}