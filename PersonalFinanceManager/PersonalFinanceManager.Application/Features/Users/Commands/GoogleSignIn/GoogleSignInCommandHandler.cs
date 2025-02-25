using Google.Apis.Auth;
using MediatR;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Domain.Models;

namespace PersonalFinanceManager.Application.Features.Users.Commands.GoogleSignIn;

public class GoogleSignInCommandHandler : IRequestHandler<GoogleSignInCommand, TokenDto>
{
    private readonly IUserService _userService;
    public GoogleSignInCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<TokenDto> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleSignInRequest.IdToken);
        var user = await _userService.GetUserByEmail(payload.Email);

        if (user is null)
        {
            user = new AppUser
            {
                UserName = payload.Name,
                Email = payload.Email,
                EmailConfirmed = true
            };

            await _userService.EnsureUserExistsAsync(user);
        }

        return await _userService.GenerateTokenAsync(user);
    }
}
