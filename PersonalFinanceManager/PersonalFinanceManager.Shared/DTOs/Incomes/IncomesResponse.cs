using PersonalFinanceManager.Shared.RequestFeatures;

namespace PersonalFinanceManager.Shared.DTOs.Incomes;

public record IncomesResponse(IEnumerable<IncomeResponseDto> Incomes, MetaData MetaData);
