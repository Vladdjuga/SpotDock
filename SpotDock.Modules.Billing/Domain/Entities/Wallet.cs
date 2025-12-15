namespace SpotDock.Modules.Billing.Domain.Entities;

public sealed class Wallet
{
    public required Guid Id { get; init; }

    public required Guid UserId { get; init; }

    public required decimal Balance { get; set; }

    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
}
