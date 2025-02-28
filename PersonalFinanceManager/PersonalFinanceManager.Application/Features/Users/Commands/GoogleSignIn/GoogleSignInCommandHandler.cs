using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Application.Helpers;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Domain.Exceptions;
using PersonalFinanceManager.Domain.Models;
using PersonalFinanceManager.Shared.Constants;

namespace PersonalFinanceManager.Application.Features.Users.Commands.GoogleSignIn;

public class GoogleSignInCommandHandler : IRequestHandler<GoogleSignInCommand, TokenDto>
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<GoogleSignInCommandHandler> _logger;
    public GoogleSignInCommandHandler(IUserService userService, ILogger<GoogleSignInCommandHandler> logger,
        UserManager<AppUser> userManager)
    {
        _userService = userService;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<TokenDto> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleSignInRequest.IdToken);
        if (payload is null)
        {
            _logger.LogError("Invalid Google token");
            throw new UnauthorizedAccessException("Invalid Google token");
        }

        AppUser user;

        try
        {
            user = await _userService.GetUserByEmail(payload.Email);
        }
        catch (NotFoundException)
        {
            _logger.LogInformation("User with email {Email} not found. Creating new user", payload.Email);
            user = new AppUser
            {
                UserName = payload.Email,
                Email = payload.Email,
                EmailConfirmed = true,
            };

            user = await _userService.EnsureUserExistsAsync(user);
            await _userManager.AddRoleToUserAsync(Roles.User, user, _logger);
        }

        _logger.LogInformation("Generating token for user: {Email}", user.Email);
        return await _userService.GenerateTokenAsync(user);
    }
}
