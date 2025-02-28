namespace PersonalFinanceManager.Domain.Entities;

public class Income
{
    public Guid IncomeId { get; set; }
    public required string Source { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; }
}