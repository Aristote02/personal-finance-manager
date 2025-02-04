using System.Text.Json;

namespace PersonalFinanceManager.Domain.ErrorModel;

/// <summary>
/// Represents an error details class
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Represents the HTTP status code
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// Contains an optional error message
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Overrides the ToString method to serialize the object
    /// </summary>
    /// <returns></returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}
