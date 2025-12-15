namespace SpotDock.Modules.Auth.Application.DTO;

public record RegisterRequest(string Email, string DisplayName, string Password);
public record LoginRequest(string Email, string Password);
