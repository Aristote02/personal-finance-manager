namespace PersonalFinanceManager.Domain.Models;

/// <summary>
/// Represent the Data Transfer Object of the token
/// </summary>
public class TokenDto
{
    /// <summary>
    /// Represents the unique identifier for the token
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The username of a particular user
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    /// Represents the email of the user
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Represents the role of the user
    /// </summary>
    public required string Role { get; init; }

    /// <summary>
    /// The jwt token
    /// </summary>
    public required string AccessToken { get; init; }

    /// <summary>
    /// The jwt refresh token
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The duration of the token in minutes
    /// </summary>
    public required int DurationInMinutes { get; init; }
}
