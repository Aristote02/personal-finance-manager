namespace PersonalFinanceManager.Shared.RequestFeatures;

public class IncomeParameters : RequestParameters
{
    public Guid? UserId { get; init; }

    public string? Source { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public decimal? MinAmount { get; set; }

    public decimal? MaxAmount { get; set; }

    public string? SearchTerm { get; init; }
}