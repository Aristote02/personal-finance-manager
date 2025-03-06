using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;
using PersonalFinanceManager.Infrastructure.Data;
using PersonalFinanceManager.Infrastructure.Repositories.Extensions;
using PersonalFinanceManager.Shared.RequestFeatures;

namespace PersonalFinanceManager.Infrastructure.Repositories.Implementations;

public class IncomeRepository : RepositoryBase<Income>, IIncomeRepository
{
    public IncomeRepository(PersonalFinanceDbContext dbContext) 
        : base(dbContext)
    {
    }

    public void CreateIncome(Income income) => Create(income);

    public void DeleteIncome(Income income) => Delete(income);

    public async Task<PagedList<Income>> GetAllIncomesAsync(IncomeParameters incomeParameters, bool trackChanges)
    {
        var query = FindAll(trackChanges)
            .ApplyUserFilter(incomeParameters.UserId)
            .ApplySourceFilter(incomeParameters.Source)
            .ApplyDateRangeFilter(incomeParameters.StartDate, incomeParameters.EndDate)
            .ApplyAmountFilter(incomeParameters.MinAmount, incomeParameters.MaxAmount)
            .SearchIncomes(incomeParameters.SearchTerm);

        var count = await query.CountAsync();

        if (count == 0)
            return new PagedList<Income>([], 0, incomeParameters.PageNumber, incomeParameters.PageSize);

        var incomes = await query
            .OrderBy(i => i.Date)
            .Skip((incomeParameters.PageNumber - 1) * incomeParameters.PageSize)
            .Take(incomeParameters.PageSize)
            .ToListAsync();

        return new PagedList<Income>(incomes, count, incomeParameters.PageNumber, incomeParameters.PageSize);
    }

    public async Task<Income?> GetIncomeByIdAsync(Guid id, bool trackChanges)
    {
        return await FindAll(trackChanges)
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
    }
}