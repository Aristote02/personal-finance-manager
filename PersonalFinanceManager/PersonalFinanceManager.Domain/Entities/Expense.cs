namespace PersonalFinanceManager.Domain.Entities;

public class Expense : BaseEntity
{
    public required string Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public required string Category { get; set; }

    public Guid UserId { get; set; }

    public AppUser User { get; set; }
}