namespace PersonalFinanceManager.Shared.Requests.Auth;

/// <summary>
/// Represent the signIn request that will be sent by the user
/// </summary>
public class UserSignInRequest
{
    // <summary>
    /// Represents a user's email
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Represents a user's password
    /// </summary>
    public required string Password { get; init; }
}