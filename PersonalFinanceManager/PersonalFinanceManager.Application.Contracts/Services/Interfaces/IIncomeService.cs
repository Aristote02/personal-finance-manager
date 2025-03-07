using PersonalFinanceManager.Shared.DTOs.Incomes;
using PersonalFinanceManager.Shared.RequestFeatures;
using PersonalFinanceManager.Shared.Requests.Incomes;

namespace PersonalFinanceManager.Application.Contracts.Services.Interfaces;

public interface IIncomeService
{
    Task<IncomesResponse> GetAllIncomesAsync(IncomeParameters incomeParameters, bool trackChanges);

    Task<IncomeResponseDto> GetIncomeByIdAsync(Guid id, bool trackChanges);

    Task<IncomeResponseDto> CreateIncomeAsync(IncomeCreationRequest incomeCreationRequest);

    Task UpdateIncomeAsync(IncomeUpdateRequest incomeUpdateRequest, bool trackChanges);

    Task DeleteIncomeAsync(Guid id, bool trackChanges);
}