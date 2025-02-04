using MediatR;
using PersonalFinanceManager.Domain.Models;
using PersonalFinanceManager.Shared.Requests.Auth;

namespace PersonalFinanceManager.Application.Features.Users.Commands.SignIn;

public sealed record UserSignInCommand(UserSignInRequest UserSignInRequest) : IRequest<TokenDto>;