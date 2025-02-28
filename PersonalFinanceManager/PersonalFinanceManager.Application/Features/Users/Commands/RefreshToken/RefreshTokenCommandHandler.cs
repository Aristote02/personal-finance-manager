using MediatR;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Domain.Models;

namespace PersonalFinanceManager.Application.Features.Users.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenDto>
{
    private readonly IUserService _userService;
    public RefreshTokenCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RefreshTokenAsync(request.Token);
    }
}
