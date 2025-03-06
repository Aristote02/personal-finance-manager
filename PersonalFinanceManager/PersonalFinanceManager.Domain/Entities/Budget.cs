namespace PersonalFinanceManager.Domain.Entities;

public class Budget : BaseEntity
{
    public required string Category { get; set; }

    public decimal Amount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid UserId { get; set; }

    public AppUser User { get; set; }
}