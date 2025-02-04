using Microsoft.AspNetCore.Identity;

namespace PersonalFinanceManager.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<Expense> Expenses { get; set; } = []!;
    public ICollection<Income> Incomes { get; set; } = []!;
    public ICollection<SavingsGoal> SavingsGoals { get; set; } = []!;
    public ICollection<Budget> Budgets { get; set; } = []!;
    public ICollection<Notification> Notifications { get; set; } = []!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = []!;
}