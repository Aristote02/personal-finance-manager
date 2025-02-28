using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Application.Helpers;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Domain.Exceptions;
using PersonalFinanceManager.Domain.Models;
using PersonalFinanceManager.Shared.Constants;

namespace PersonalFinanceManager.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, TokenDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IUserService _userService;
    public CreateUserCommandHandler(UserManager<AppUser> userManager,
        ILogger<CreateUserCommandHandler> logger,
        IMapper mapper,
        IValidator<CreateUserCommand> validator,
        IUserService userService)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
        _userService = userService;
    }

    public async Task<TokenDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation failed: {ValidationErrors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }

        if (await _userManager.IsUserEmailExist(request.UserRegisterRequest.Email))
        {
            _logger.LogError("A user with the email {Email} already exists", request.UserRegisterRequest.Email);
            throw new AlreadyExistsException($"A user with the email {request.UserRegisterRequest.Email} already exists");
        }

        var identityUser = _mapper.Map<AppUser>(request.UserRegisterRequest);
        identityUser.Id = Guid.NewGuid();
        identityUser.EmailConfirmed = true;

        var result = await _userManager.CreateAsync(identityUser, request.UserRegisterRequest.Password);
        result.ThrowExceptionIfResultDoNotSucceed(_logger);

        var user = await _userManager.AddRoleToUserAsync(Roles.User, identityUser, _logger);

        return await _userService.GenerateTokenAsync(user);
    }
}