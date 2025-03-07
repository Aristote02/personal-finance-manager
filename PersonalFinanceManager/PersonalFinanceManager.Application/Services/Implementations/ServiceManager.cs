using PersonalFinanceManager.Application.Contracts.Services.Interfaces;

namespace PersonalFinanceManager.Application.Services.Implementations;

public sealed class ServiceManager : IServiceManager
{
    private readonly IIncomeService _incomeService;

    public ServiceManager(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }

    public IIncomeService IncomeService => _incomeService;
}
