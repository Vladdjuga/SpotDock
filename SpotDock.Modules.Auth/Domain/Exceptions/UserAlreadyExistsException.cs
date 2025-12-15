namespace SpotDock.Modules.Auth.Domain.Exceptions;

public sealed class UserAlreadyExistsException(string email) : Exception($"User with email '{email}' already exists.")
{
    public string Email { get; } = email;
}
