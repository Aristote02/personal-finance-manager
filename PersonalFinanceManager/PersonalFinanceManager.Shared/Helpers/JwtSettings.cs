namespace PersonalFinanceManager.Shared.Helpers;

/// <summary>
/// Represents settings related to JSON Web Tokens (JWT).
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets the secret key used for signing JWTs
    /// </summary>
    public required string Key { get; init; }

    /// <summary>
    /// Gets the audience for which the JWTs are intended
    /// </summary>
    public required string Audience { get; init; }

    /// <summary>
    /// Gets the issuer of the JWTs
    /// </summary>
    public required string Issuer { get; init; }

    /// <summary>
    /// Gets the duration of the JWTs in minutes
    /// </summary>
    public required int DurationInMinutes { get; init; }
}