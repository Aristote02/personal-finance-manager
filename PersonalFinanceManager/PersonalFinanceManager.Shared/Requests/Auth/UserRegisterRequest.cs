namespace PersonalFinanceManager.Shared.Requests.Auth;

/// <summary>
/// The register request which the user will send
/// </summary>
public class UserRegisterRequest
{
    /// <summary>
    /// The user's register Username
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    /// The user's register email
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The user's register password 
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// The user's register password confirmation
    /// </summary>
    public required string ConfirmPassword { get; init; }
}