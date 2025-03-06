namespace PersonalFinanceManager.Domain.Entities;

public abstract class BaseEntity
{
    public required Guid Id { get; init; }
}