namespace SpotDock.Modules.Auth.Infrastructure.Security;

public sealed class PasswordHasherOptions
{
    public const string SectionName = "PasswordHashing";

    public int SaltSize { get; init; } = 16;       // bytes
    public int KeySize { get; init; } = 32;        // bytes
    public int Iterations { get; init; } = 100_000;
}
