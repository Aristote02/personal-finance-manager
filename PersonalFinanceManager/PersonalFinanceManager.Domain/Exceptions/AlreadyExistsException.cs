namespace PersonalFinanceManager.Domain.Exceptions;

/// <summary>
/// Represents error that occurs when a requested resource already exist
/// </summary>
public class AlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="AlreadyExistsException"/>
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public AlreadyExistsException(string message) : base(message) { }
}