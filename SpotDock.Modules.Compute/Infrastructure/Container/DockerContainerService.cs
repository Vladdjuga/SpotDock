using Docker.DotNet;
using Docker.DotNet.Models;
using SpotDock.Modules.Compute.Application.Interfaces;

namespace SpotDock.Modules.Compute.Infrastructure.Container;

public class DockerContainerService(IDockerClient client) : IContainerService
{
    public async Task PullImageAsync(string image, string tag, CancellationToken ct)
    {
        await client.Images.CreateImageAsync(
            new ImagesCreateParameters { FromImage = image, Tag = tag },
            null,
            new Progress<JSONMessage>(),
            ct
        );
    }

    public async Task<string> StartContainerAsync(string image, long memoryLimitBytes, double cpuLimit, CancellationToken ct)
    {
        var hostConfig = new HostConfig
        {
            Memory = memoryLimitBytes,
            NanoCPUs = (long)(cpuLimit * 1_000_000_000)
        };
        
        var response = await client.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = image,
                HostConfig = hostConfig
            }, ct);
        
        await client.Containers.StartContainerAsync(response.ID,new ContainerStartParameters(), ct);

        return response.ID;
    }

    public async Task StopContainerAsync(string containerId, CancellationToken ct)
    {
        await client.Containers.KillContainerAsync(containerId, new ContainerKillParameters(), ct);
    }
}