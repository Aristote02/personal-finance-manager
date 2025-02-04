using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Domain.Models;

namespace PersonalFinanceManager.Application.Contracts.Services.Interfaces;

/// <summary>
/// Interface for user services
/// </summary>
public interface IUserService
{
    /// <summary>
    /// generates a token for a given user account
    /// </summary>
    /// <param name="user">The user for whom the token is to be generated</param>
    /// <returns>TokenDto as the result</returns>
    Task<TokenDto> GenerateTokenAsync(AppUser user);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<TokenDto> RefreshTokenAsync(string token);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task RevokeRefreshTokensAsync(Guid userId);

    /// <summary>
    /// retrieves a user by their email
    /// </summary>
    /// <param name="email">The email of the user to be retrieved</param>
    /// <returns>A task representing the asynchronous operation with an IdentityUser as the result</returns>
    Task<AppUser> GetUserByEmail(string email);

    /// <summary>
    /// Asynchronously checks if the provided password matches the user's password
    /// </summary>
    /// <param name="user">The user whose password is to be checked</param>
    /// <param name="password">The password to check against the user's stored password</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CheckPassword(AppUser user, string password);

    /// <summary>
    /// Method to ensures the user's email is confirmed
    /// </summary>
    /// <param name="user">The user whose email confirmation status is to be checked</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task EnsureEmailConfirmed(AppUser user);

    /// <summary>
    /// validates a given request
    /// </summary>
    /// <typeparam name="T">The type of the request object</typeparam>
    /// <param name="request">The request object to validate</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task ValidateRequest<T>(T request, CancellationToken cancellationToken) where T : class;
}