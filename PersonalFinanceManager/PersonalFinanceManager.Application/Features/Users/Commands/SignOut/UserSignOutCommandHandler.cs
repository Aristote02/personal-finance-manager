using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Domain.Entities;

namespace PersonalFinanceManager.Application.Features.Users.Commands.SignOut;

/// <summary>
/// Handles the user sign out
/// </summary>
public class UserSignOutCommandHandler : IRequestHandler<UserSignOutCommand>
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<UserSignOutCommandHandler> _logger;
    private readonly IUserService _userService;

    public UserSignOutCommandHandler(SignInManager<AppUser> signInManager,
        ILogger<UserSignOutCommandHandler> logger,
        IUserService userService)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userService = userService;
    }

    public async Task Handle(UserSignOutCommand request, CancellationToken cancellationToken)
    {
        await _userService.RevokeRefreshTokensAsync(request.UserId);

        await _signInManager.SignOutAsync();
        _logger.LogInformation("user with id {UserId} signed out successfully", request.UserId);
    }
}