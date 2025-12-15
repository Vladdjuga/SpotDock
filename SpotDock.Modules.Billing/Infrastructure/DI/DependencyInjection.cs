using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotDock.Modules.Billing.Domain.Repositories;
using SpotDock.Modules.Billing.Infrastructure.Persistence;
using SpotDock.Modules.Billing.Infrastructure.Persistence.Context;

namespace SpotDock.Modules.Billing.Infrastructure.DI;

public static class DependencyInjection
{
    public static void AddBillingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BillingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("billingDb"))
        );

        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
