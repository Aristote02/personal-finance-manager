namespace PersonalFinanceManager.Domain.Exceptions;

/// <summary>
/// Represents an exception that occurs when an operation is unauthorized
/// </summary>
public class UnauthorizedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with the specified error message
    /// </summary>
    /// <param name="message"></param>
    public UnauthorizedException(string message) : base(message) { }
}
