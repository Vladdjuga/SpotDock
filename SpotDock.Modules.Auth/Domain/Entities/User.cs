namespace SpotDock.Modules.Auth.Domain.Entities;

public sealed class User
{
    public required Guid Id { get; init; }

    public required string Email { get; set; }
    public required string DisplayName { get; set; }

    public string? PasswordHash { get; set; }

    public required DateTime CreatedAt { get; init; }
    public DateTime? LastLoginAt { get; set; }
}
