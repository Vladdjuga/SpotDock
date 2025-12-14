using Docker.DotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpotDock.Modules.Compute.Infrastructure.DI;

public static class DependencyInjection
{
    public static void AddComputeModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDockerClient>(_ =>
        {
            var config = new DockerClientConfiguration(
                new Uri(configuration["Docker:Engine:Path"] ?? string.Empty) // TODO : Make a config class for this
            );
            return config.CreateClient();
        });
    }
}