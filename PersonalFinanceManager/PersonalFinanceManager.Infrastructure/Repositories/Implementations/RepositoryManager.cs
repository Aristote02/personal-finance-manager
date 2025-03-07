using PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;
using PersonalFinanceManager.Infrastructure.Data;

namespace PersonalFinanceManager.Infrastructure.Repositories.Implementations;

public class RepositoryManager : IRepositoryManager
{
    private readonly PersonalFinanceDbContext _context;
    private readonly IIncomeRepository _incomeRepository;

    public RepositoryManager(PersonalFinanceDbContext context, 
        IIncomeRepository incomeRepository)
    {
        _context = context;
        _incomeRepository = incomeRepository;
    }

    public IIncomeRepository IncomeRepository => _incomeRepository;

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}