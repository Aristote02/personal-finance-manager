using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Domain.Exceptions;
using PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;
using PersonalFinanceManager.Shared.DTOs.Incomes;
using PersonalFinanceManager.Shared.RequestFeatures;
using PersonalFinanceManager.Shared.Requests.Incomes;

namespace PersonalFinanceManager.Application.Services.Implementations;

public sealed class IncomeService : IIncomeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IValidator<IncomeCreationRequest> _incomeCreationRequestValidator;
    private readonly IValidator<IncomeUpdateRequest> _incomeUpdateRequestValidator;
    private readonly ILogger<IncomeService> _logger;

    public IncomeService(IRepositoryManager repositoryManager,
        IMapper mapper,
        IValidator<IncomeCreationRequest> incomeCreationRequestValidator,
        IValidator<IncomeUpdateRequest> incomeUpdateRequestValidator,
        ILogger<IncomeService> logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _incomeCreationRequestValidator = incomeCreationRequestValidator;
        _incomeUpdateRequestValidator = incomeUpdateRequestValidator;
        _logger = logger;
    }

    public async Task<IncomeResponseDto> CreateIncomeAsync(IncomeCreationRequest incomeCreationRequest)
    {
        await ValidateIncomeRequest(incomeCreationRequest, _incomeCreationRequestValidator);
        var income = _mapper.Map<Income>(incomeCreationRequest);
        _repositoryManager.IncomeRepository.CreateIncome(income);
        await _repositoryManager.SaveAsync();
        return _mapper.Map<IncomeResponseDto>(income);
    }

    public async Task<IncomeResponseDto> GetIncomeByIdAsync(Guid id, bool trackChanges)
    {
        var income = await GetIncomeOrThrowIfNotFound(id, trackChanges);
        return _mapper.Map<IncomeResponseDto>(income);
    }

    public async Task<IncomesResponse> GetAllIncomesAsync(IncomeParameters incomeParameters, bool trackChanges)
    {
        var doctors = await _repositoryManager.IncomeRepository.GetAllIncomesAsync(incomeParameters, trackChanges);
        return _mapper.Map<IncomesResponse>(doctors);
    }

    public async Task UpdateIncomeAsync(IncomeUpdateRequest incomeUpdateRequest, bool trackChanges)
    {
        await ValidateIncomeRequest(incomeUpdateRequest, _incomeUpdateRequestValidator);
        var income = await GetIncomeOrThrowIfNotFound(incomeUpdateRequest.Id, trackChanges);
        _mapper.Map(incomeUpdateRequest, income);
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteIncomeAsync(Guid id, bool trackChanges)
    {
        var income = await GetIncomeOrThrowIfNotFound(id, trackChanges);
        _repositoryManager.IncomeRepository.DeleteIncome(income);
        await _repositoryManager.SaveAsync();
    }

    private async Task<Income> GetIncomeOrThrowIfNotFound(Guid id, bool trackChanges)
    {
        var income = await _repositoryManager.IncomeRepository.GetIncomeByIdAsync(id, trackChanges);
        if (income is null)
        {
            _logger.LogError("Income with id {Id} was not found", id);
            throw new NotFoundException($"Income with id {id} was not found");
        }
        return income;
    }

    private async Task ValidateIncomeRequest<T>(T request, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation failed: {Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }
    }
}