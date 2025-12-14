using SpotDock.Shared.Kernel.Domain;

namespace SpotDock.Modules.Auctions.Domain.Events;

/// <summary>
/// Raised when a bid wins a spot instance and gets matched.
/// Pure domain event, internal to the Auctions module.
/// </summary>
public sealed record BidMatchedDomainEvent(
    Guid BidId,
    Guid UserId,
    Guid SpotInstanceId,
    decimal ClearingPrice
) : DomainEvent;
