using MediatR;
using PersonalFinanceManager.Domain.Models;
using PersonalFinanceManager.Shared.Requests.Auth;

namespace PersonalFinanceManager.Application.Features.Users.Commands.GoogleSignIn;

public sealed record GoogleSignInCommand(GoogleSignInRequest GoogleSignInRequest) : IRequest<TokenDto>;