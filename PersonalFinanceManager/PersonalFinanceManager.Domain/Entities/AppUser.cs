using Microsoft.AspNetCore.Identity;

namespace PersonalFinanceManager.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<Expense> Expenses { get; init; } = []!;
    public ICollection<Income> Incomes { get; init; } = []!;
    public ICollection<SavingsGoal> SavingsGoals { get; init; } = []!;
    public ICollection<Budget> Budgets { get; init; } = []!;
    public ICollection<Notification> Notifications { get; init; } = []!;
    public ICollection<RefreshToken> RefreshTokens { get; init; } = []!;
}