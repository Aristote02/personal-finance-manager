namespace PersonalFinanceManager.Shared.DTOs.Incomes;

public record IncomeResponseDto(Guid Id, string Source, decimal Amount, DateTime Date, Guid UserId);