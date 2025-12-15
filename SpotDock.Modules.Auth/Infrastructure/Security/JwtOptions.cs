namespace SpotDock.Modules.Auth.Infrastructure.Security;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SecretKey { get; init; }
    public int AccessTokenLifetimeMinutes { get; init; } = 60;
}
