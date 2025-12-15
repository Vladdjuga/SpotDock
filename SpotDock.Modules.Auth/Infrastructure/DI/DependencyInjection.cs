using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotDock.Modules.Auth.Application.Interfaces;
using SpotDock.Modules.Auth.Domain.Repositories;
using SpotDock.Modules.Auth.Infrastructure.Persistence;
using SpotDock.Modules.Auth.Infrastructure.Persistence.Context;
using SpotDock.Modules.Auth.Infrastructure.Security;

namespace SpotDock.Modules.Auth.Infrastructure.DI;

public static class DependencyInjection
{
    public static void AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("authDb"))
        );

        services.AddScoped<IUserRepository, UserRepository>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.Configure<PasswordHasherOptions>(configuration.GetSection(PasswordHasherOptions.SectionName));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
