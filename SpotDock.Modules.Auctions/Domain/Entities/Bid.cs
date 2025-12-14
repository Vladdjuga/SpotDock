namespace SpotDock.Modules.Auctions.Domain.Entities;

public sealed class Bid
{
    public required Guid Id { get; init; }
    
    public required SpotInstance SpotInstance { get; set; }
    public required Guid SpotInstanceId { get; set; } 
        
    public required Guid UserId { get; set; }
    
    public required decimal Amount { get; set; }
    public required DateTime CreatedAt { get; set; }
}