using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PersonalFinanceManager.Domain.Entities;
using System.Text;

namespace PersonalFinanceManager.Application.Helpers;

public static class UserHelper
{
    /// <summary>
    /// Throws an exception if the <see cref="IdentityResult"/> does not indicate success, logging errors if present
    /// </summary>
    /// <param name="identityResult">The <see cref="IdentityResult"/>to check</param>
    /// <param name="logger">The <see cref="ILogger"/> to log errors if present</param>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="IdentityResult"/>indicates failure</exception>
    public static void ThrowExceptionIfResultDoNotSucceed(this IdentityResult identityResult, ILogger logger)
    {
        if (identityResult.Succeeded)
        {
            return;
        }

        var cause = identityResult.Errors.Select(x => x.Description)
            .Aggregate(new StringBuilder(), (builder, description) => builder.Append(description));

        logger.LogError("Identity operation failed: {cause}", cause);

        throw new InvalidOperationException(cause.ToString());
    }

    /// <summary>
    /// Adds a role to a user asynchronously and throws an exception if the operation fails, logging errors if present
    /// </summary>
    /// <typeparam name="T">Type of the user (should be derived from IdentityUser)</typeparam>
    /// <param name="userManager">The UserManager used to manage users</param>
    /// <param name="role">The role to add to the user</param>
    /// <param name="user">The user to whom the role will be added</param>
    /// <param name="logger">The logger to log errors if present</param>
    /// <returns>The user after the role is added</returns>
    public static async Task<T> AddRoleToUserAsync<T>(this UserManager<T> userManager, string role, T user, ILogger logger) where T : AppUser
    {
        var roleResult = await userManager.AddToRoleAsync(user, role);

        roleResult.ThrowExceptionIfResultDoNotSucceed(logger);

        return user;
    }

    /// <summary>
    /// Checks if a user with the given email exists
    /// </summary>
    /// <typeparam name="T">Type of the user (should be derived from IdentityUser)</typeparam>
    /// <param name="userManager">The UserManager used to manage users</param>
    /// <param name="email">The email to check for existence</param>
    /// <returns>True if a user with the given email exists, otherwise false</returns>
    public static async Task<bool> IsUserEmailExist<T>(this UserManager<T> userManager, string email) where T : AppUser
    {
        var userLooked = await userManager.FindByEmailAsync(email);

        return userLooked is not null;
    }
}