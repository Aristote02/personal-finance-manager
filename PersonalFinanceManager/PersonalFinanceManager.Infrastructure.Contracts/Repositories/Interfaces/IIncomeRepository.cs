using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Shared.RequestFeatures;

namespace PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;

public interface IIncomeRepository
{
    Task<PagedList<Income>> GetAllIncomesAsync(IncomeParameters incomeParameters, bool trackChanges);

    Task<Income?> GetIncomeByIdAsync(Guid id, bool trackChanges);

    void CreateIncome(Income income);

    void DeleteIncome(Income income);
}