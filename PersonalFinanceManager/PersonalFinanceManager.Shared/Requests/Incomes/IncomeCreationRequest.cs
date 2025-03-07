namespace PersonalFinanceManager.Shared.Requests.Incomes;

public class IncomeCreationRequest
{
    public required string Source { get; init; }

    public required decimal Amount { get; init; }

    public DateTime Date { get; init; }

    public Guid UserId { get; set; }
}