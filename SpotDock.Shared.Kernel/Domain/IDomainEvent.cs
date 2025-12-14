namespace SpotDock.Shared.Kernel.Domain;

/// <summary>
/// Marker interface for domain events raised by aggregates.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
