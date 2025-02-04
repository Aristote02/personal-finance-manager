using MediatR;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Domain.Models;

namespace PersonalFinanceManager.Application.Features.Users.Commands.SignIn;

public class UserSignInCommandHandler : IRequestHandler<UserSignInCommand, TokenDto>
{
    private readonly IUserService _userService;
    public UserSignInCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<TokenDto> Handle(UserSignInCommand request, CancellationToken cancellationToken)
    {
        await _userService.ValidateRequest(request, cancellationToken);
        var user = await _userService.GetUserByEmail(request.UserSignInRequest.Email);
        await _userService.CheckPassword(user, request.UserSignInRequest.Password);
        await _userService.EnsureEmailConfirmed(user);

        return await _userService.GenerateTokenAsync(user);
    }
}