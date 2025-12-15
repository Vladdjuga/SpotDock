using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpotDock.Modules.Auth.Application.Interfaces;
using SpotDock.Modules.Auth.Domain.Entities;

namespace SpotDock.Modules.Auth.Infrastructure.Security;

public sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateAccessToken(User user, IReadOnlyCollection<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("name", user.DisplayName)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetExpiration(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        if (handler.ReadToken(token) is not JwtSecurityToken jwt)
        {
            throw new ArgumentException("Invalid JWT token", nameof(token));
        }

        return jwt.ValidTo;
    }
}
