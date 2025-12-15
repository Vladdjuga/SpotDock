using SpotDock.Modules.Market.Domain.Enums;

namespace SpotDock.Modules.Market.Domain.Entities;

public sealed class SpotInstance
{
    public required Guid Id { get; init; }
    
    // Characteristics
    public uint CpuCores { get; private set; }
    public uint RamMb { get; private set; }
    
    // Current charging price per hour
    public required decimal CurrentPrice { get; set; }
    public required DateTime EndsAt { get; set; }
    public Guid? OwnerId { get; set; }
    public required SpotInstanceStatus Status { get; set; }
}