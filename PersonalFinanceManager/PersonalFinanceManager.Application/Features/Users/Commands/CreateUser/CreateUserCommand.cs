using MediatR;
using PersonalFinanceManager.Domain.Models;
using PersonalFinanceManager.Shared.Requests.Auth;

namespace PersonalFinanceManager.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(UserRegisterRequest UserRegisterRequest) : IRequest<TokenDto>;