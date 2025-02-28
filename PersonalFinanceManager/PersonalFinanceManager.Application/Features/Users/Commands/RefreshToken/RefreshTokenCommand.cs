using MediatR;
using PersonalFinanceManager.Domain.Models;

namespace PersonalFinanceManager.Application.Features.Users.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string Token) : IRequest<TokenDto>;