namespace PersonalFinanceManager.Shared.Requests.Incomes;

public class IncomeUpdateRequest
{
    public Guid Id { get; init; }

    public required string Source { get; init; }

    public required decimal Amount { get; init; }

    public DateTime Date { get; set; }

    public Guid UserId { get; set; }
}