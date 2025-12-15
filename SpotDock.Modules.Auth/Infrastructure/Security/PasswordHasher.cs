using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using SpotDock.Modules.Auth.Application.Interfaces;

namespace SpotDock.Modules.Auth.Infrastructure.Security;

public sealed class PasswordHasher(IOptions<PasswordHasherOptions> options) : IPasswordHasher
{
    private readonly PasswordHasherOptions _options = options.Value;
    private const char Delimiter = ':';

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[_options.SaltSize];
        rng.GetBytes(salt);

        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            _options.Iterations,
            HashAlgorithmName.SHA256,
            _options.KeySize);

        return string.Join(Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(key));
    }

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        var parts = passwordHash.Split(Delimiter);
        if (parts.Length != 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var expectedKey = Convert.FromBase64String(parts[1]);

        var actualKey = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            _options.Iterations,
            HashAlgorithmName.SHA256,
            _options.KeySize);

        return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
    }
}
