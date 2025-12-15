using SpotDock.Modules.Auth.Domain.Entities;

namespace SpotDock.Modules.Auth.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateAccessToken(User user, IReadOnlyCollection<string> roles);
    DateTime GetExpiration(string token);
}
