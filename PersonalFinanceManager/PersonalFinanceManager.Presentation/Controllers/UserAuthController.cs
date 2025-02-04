using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManager.Application.Features.Users.Commands.CreateUser;
using PersonalFinanceManager.Application.Features.Users.Commands.RefreshToken;
using PersonalFinanceManager.Application.Features.Users.Commands.SignIn;
using PersonalFinanceManager.Application.Features.Users.Commands.SignOut;
using PersonalFinanceManager.Shared.Requests.Auth;

namespace PersonalFinanceManager.Presentation.Controllers;

[Route("api/auth")]
[ApiController]
public class UserAuthController : ControllerBase
{
    private readonly ISender _sender;
    public UserAuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUp(UserRegisterRequest request)
    {
        var tokenDto = await _sender.Send(new CreateUserCommand(request));

        return CreatedAtAction(nameof(SignUp), tokenDto);
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn(UserSignInRequest request)
    {
        var tokenDto = await _sender.Send(new UserSignInCommand(request));
        if (!string.IsNullOrEmpty(tokenDto.RefreshToken))
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };

            Response.Cookies.Append("refreshToken", tokenDto.RefreshToken, cookieOptions);
        }

        return CreatedAtAction(nameof(SignIn), tokenDto);
    }

    /// <summary>
	/// Sign-out endpoint
	/// </summary>
	/// <returns></returns>
	[HttpPost("sign-out")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UserSignOut([FromHeader] Guid userId)
    {
        await _sender.Send(new UserSignOutCommand(userId));

        return NoContent();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(string token)
    {
        var tokenDto = await _sender.Send(new RefreshTokenCommand(token));

        return Ok(tokenDto);
    }
}
