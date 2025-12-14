namespace SpotDock.Modules.Compute.Application.Interfaces;

public interface IContainerService
{
    public Task PullImageAsync(string image, string tag, CancellationToken ct);
    public Task<string> StartContainerAsync(string image,
        long memoryLimitBytes,
        double cpuLimit,
        CancellationToken ct);

    public Task StopContainerAsync(string containerId, CancellationToken ct);
}