using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Application.Features.Users.Commands.SignIn;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Domain.Exceptions;
using PersonalFinanceManager.Domain.Models;
using PersonalFinanceManager.Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PersonalFinanceManager.Application.Services.Implementations;

/// <summary>
/// Handles the logic of the user operations
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<UserService> _logger;
    private readonly IValidator<UserSignInCommand> _userValidator;

    /// <summary>
    /// Initializes a new instance of <see cref="UserService"/>
    /// </summary>
    /// <param name="userManager">User manager for managing user accounts</param>
    /// <param name="jwtSettings">JWT settings for token generation</param>
    /// <param name="logger">Logger for logging information and errors</param>
    /// <param name="userValidator">Validator for user sign-in commands</param>
    /// </summary>
    public UserService(UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings, ILogger<UserService> logger,
        IValidator<UserSignInCommand> userValidator)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
        _userValidator = userValidator;
    }

    /// <summary>
    /// Method to verify the user's password
    /// </summary>
    /// <param name="user">User's data</param>
    /// <param name="password">user's password request</param>
    /// <returns></returns>
    /// <exception cref="InvalidCredentialsException"></exception>
    public async Task CheckPassword(AppUser user, string password)
    {
        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            _logger.LogError("Invalid password or email");
            throw new InvalidCredentialsException("Wrong email or Password, please try again or sign up");
        }
    }

    /// <summary>
    /// Method to verify the user's email
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedException"></exception>
    public async Task EnsureEmailConfirmed(AppUser user)
    {
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            _logger.LogError("The user email {Email} is not confirmed", user.Email);
            throw new UnauthorizedException($"The email {user.Email} of the user is not confirmed");
        }
    }

    /// <summary>
    /// Method to retrieve the user by their email
    /// </summary>
    /// <param name="email">request</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<AppUser> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            _logger.LogError("There is no user with that email {Email}", email);
            throw new NotFoundException("This user does not exist");
        }

        return user;
    }

    /// <summary>
    /// Validates the user sign-in request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public async Task ValidateRequest<T>(T request, CancellationToken cancellationToken)
        where T : class
    {
        if (request is UserSignInCommand userSignInCommand)
        {
            var validationResult = await _userValidator.ValidateAsync(userSignInCommand, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed : {ValidationErrors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
        }
    }

    /// <summary>
    /// method to generate a JWT token for the user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<TokenDto> GenerateTokenAsync(AppUser user)
    {
        if (user.Email == null || user.UserName == null)
        {
            _logger.LogError("User email or userName are null for user with Id {UserId}", user.Id);
            throw new InvalidOperationException("The user email or userName can't be null");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var userRole = await _userManager.GetRolesAsync(user);
        var role = userRole.First();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            ]),

            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtSettings.DurationInMinutes)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken
        {
            Token = GenerateSecureToken(),
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        return new TokenDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = role,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            DurationInMinutes = _jwtSettings.DurationInMinutes
        };
    }

    public async Task<TokenDto> RefreshTokenAsync(string token)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == token));

        if (user is null)
        {
            _logger.LogError("Invalid refresh token");
            throw new UnauthorizedException("Invalid refresh token");
        }

        var refreshToken = user.RefreshTokens.Single(rt => rt.Token == token);
        if (refreshToken.ExpiryDate < DateTime.UtcNow)
        {
            _logger.LogError("Expired refresh token");
            throw new UnauthorizedException("Expired refresh token");
        }

        // revoke old refresh token
        user.RefreshTokens.Remove(refreshToken);
        await _userManager.UpdateAsync(user);

        return await GenerateTokenAsync(user);
    }

    private static string GenerateSecureToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public async Task RevokeRefreshTokensAsync(Guid userId)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            _logger.LogError("User with id {UserId} not found", userId);
            throw new NotFoundException("User not found");
        }

        foreach (var refreshToken in user.RefreshTokens)
        {
            refreshToken.IsRevoked = true;
        }

        await _userManager.UpdateAsync(user);
        _logger.LogInformation("RefreshTokens revoked for user with id {UserId}", userId);
    }
}