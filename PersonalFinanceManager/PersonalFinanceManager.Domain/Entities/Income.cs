namespace PersonalFinanceManager.Domain.Entities;

public class Income : BaseEntity
{
    public required string Source { get; init; }

    public decimal Amount { get; init; }

    public DateTime Date { get; init; }

    public Guid UserId { get; init; }

    public required AppUser User { get; init; }
}