using MediatR;
using SpotDock.Modules.Auth.Application.Interfaces;
using SpotDock.Modules.Auth.Domain.Entities;
using SpotDock.Modules.Auth.Domain.Exceptions;
using SpotDock.Modules.Auth.Domain.Repositories;

namespace SpotDock.Modules.Auth.Application.Features.Users;

public static class RegisterUserFeature
{
    public sealed record Request(
        string Email,
        string DisplayName,
        string Password
    ) : IRequest<Result>;

    public sealed record Result(
        Guid UserId,
        string AccessToken
    );

    public sealed class Handler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
        : IRequestHandler<Request, Result>
    {
        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var existing = await userRepository.GetByEmailAsync(request.Email);
            if (existing is not null)
                throw new UserAlreadyExistsException(request.Email);

            var passwordHash = passwordHasher.Hash(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                DisplayName = request.DisplayName,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            await userRepository.CreateAsync(user);

            var token = jwtProvider.GenerateAccessToken(user, Array.Empty<string>());

            return new Result(user.Id, token);
        }
    }
}
