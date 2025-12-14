using SpotDock.Modules.Auctions.Domain.Enums;

namespace SpotDock.Modules.Auctions.Domain.Entities;

public sealed class SpotInstance
{
    public required Guid Id { get; init; }
    
    // Characteristics
    public int CpuCores { get; private set; }
    public int RamMb { get; private set; }
    
    public IList<Bid>? Bids { get; set; }
    
    public required decimal CurrentPrice { get; set; }
    public required DateTime EndsAt { get; set; }
    public Guid? OwnerId { get; set; }
    public required SpotInstanceStatus Status { get; set; }
}