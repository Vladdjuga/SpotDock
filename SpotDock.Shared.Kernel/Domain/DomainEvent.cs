namespace SpotDock.Shared.Kernel.Domain;

/// <summary>
/// Base implementation for domain events with OccurredOn timestamp.
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
