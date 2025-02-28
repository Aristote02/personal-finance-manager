using MediatR;

namespace PersonalFinanceManager.Application.Features.Users.Commands.SignOut;

/// <summary>
/// Represents a command to sign out the user
/// </summary>
public sealed record UserSignOutCommand(Guid UserId) : IRequest;