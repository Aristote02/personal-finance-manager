namespace PersonalFinanceManager.Domain.Exceptions;

/// <summary>
/// Represents an exception that occurs when credentials are invalid
/// </summary>
public class InvalidCredentialsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCredentialsException"/> class with the specified error message
    /// </summary>
    /// <param name="message"></param>
    public InvalidCredentialsException(string message) : base(message) { }
}