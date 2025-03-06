namespace PersonalFinanceManager.Domain.Entities;

public class SavingsGoal : BaseEntity
{
    public required string GoalName { get; set; }

    public decimal TargetAmount { get; set; }

    public decimal CurrentAmount { get; set; }

    public DateTime Deadline { get; set; }

    public Guid UserId { get; set; }

    public AppUser User { get; set; }
}