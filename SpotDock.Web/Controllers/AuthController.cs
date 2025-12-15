using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpotDock.Modules.Auth.Application.DTO;
using SpotDock.Modules.Auth.Application.Features.Users;
using SpotDock.Web.Helpers.Cookie;

namespace SpotDock.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await mediator.Send(new RegisterUserFeature.Request(
            request.Email,
            request.DisplayName,
            request.Password
        ));

        Response.Cookies.SetAuth(result.AccessToken);

        return Ok(new
        {
            userId = result.UserId,
            message = "User registered successfully"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await mediator.Send(new LoginUserFeature.Request(
            request.Email,
            request.Password
        ));

        Response.Cookies.SetAuth(result.AccessToken);

        return Ok(new
        {
            userId = result.UserId,
            message = "Login successful"
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok(new { message = "Logout successful" });
    }
}