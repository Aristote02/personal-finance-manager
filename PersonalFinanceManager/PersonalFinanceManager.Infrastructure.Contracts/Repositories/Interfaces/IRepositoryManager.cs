namespace PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;

public interface IRepositoryManager
{
    IIncomeRepository IncomeRepository { get; }
}