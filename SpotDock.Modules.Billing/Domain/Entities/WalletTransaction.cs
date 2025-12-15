using SpotDock.Modules.Billing.Domain.Enums;

namespace SpotDock.Modules.Billing.Domain.Entities;

public sealed class WalletTransaction
{
    public required Guid Id { get; init; }

    public required Guid WalletId { get; init; }

    public required decimal Amount { get; init; }

    public required WalletTransactionType Type { get; init; }

    public string? Description { get; init; }

    public required DateTime CreatedAt { get; init; }
}
