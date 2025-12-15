using MediatR;
using SpotDock.Modules.Auth.Application.Interfaces;
using SpotDock.Modules.Auth.Domain.Exceptions;
using SpotDock.Modules.Auth.Domain.Repositories;

namespace SpotDock.Modules.Auth.Application.Features.Users;

public static class LoginUserFeature
{
    public sealed record Request(
        string Email,
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
            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user is null)
                throw new InvalidCredentialsException();

            if (!passwordHasher.Verify(request.Password, user.PasswordHash ?? string.Empty))
                throw new InvalidCredentialsException();

            user.LastLoginAt = DateTime.UtcNow;
            await userRepository.UpdateAsync(user);

            var token = jwtProvider.GenerateAccessToken(user, Array.Empty<string>());

            return new Result(user.Id, token);
        }
    }
}
