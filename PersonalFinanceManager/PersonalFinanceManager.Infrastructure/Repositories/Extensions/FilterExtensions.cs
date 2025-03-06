using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Domain.Entities;

namespace PersonalFinanceManager.Infrastructure.Repositories.Extensions;

public static class FilterExtensions
{
    public static IQueryable<Budget> ApplyUserFilter(this IQueryable<Budget> budgets, Guid? userId)
    {
        return userId is null
            ? budgets
            : budgets.Where(b => b.UserId == userId);
    }

    public static IQueryable<Budget> ApplyCategoryFilter(this IQueryable<Budget> budgets, string? category)
    {
        return string.IsNullOrWhiteSpace(category)
            ? budgets
            : budgets.Where(b => b.Category == category);
    }

    public static IQueryable<Budget> ApplyDateRangeFilter(this IQueryable<Budget> budgets, DateTime? startDate, DateTime? endDate)
    {
        if (startDate is null || endDate is null)
            return budgets;

        return budgets.Where(b => b.StartDate >= startDate && b.EndDate <= endDate);
    }

    public static IQueryable<Expense> ApplyUserFilter(this IQueryable<Expense> expenses, Guid? userId)
    {
        return userId is null
            ? expenses
            : expenses.Where(e => e.UserId == userId);
    }

    public static IQueryable<Expense> ApplyCategoryFilter(this IQueryable<Expense> expenses, string? category)
    {
        return string.IsNullOrWhiteSpace(category)
            ? expenses
            : expenses.Where(e => e.Category == category);
    }

    public static IQueryable<Expense> ApplyDateRangeFilter(this IQueryable<Expense> expenses, DateTime? startDate, DateTime? endDate)
    {
        if (startDate is null || endDate is null)
            return expenses;

        return expenses.Where(e => e.Date >= startDate && e.Date <= endDate);
    }

    public static IQueryable<Expense> ApplyAmountFilter(this IQueryable<Expense> expenses, decimal? minAmount, decimal? maxAmount)
    {
        if (minAmount is null && maxAmount is null)
            return expenses;

        return expenses.Where(e => (minAmount == null || e.Amount >= minAmount) &&
                                  (maxAmount == null || e.Amount <= maxAmount));
    }

    public static IQueryable<Income> ApplyUserFilter(this IQueryable<Income> incomes, Guid? userId)
    {
        return userId is null
            ? incomes
            : incomes.Where(i => i.UserId == userId);
    }

    public static IQueryable<Income> ApplySourceFilter(this IQueryable<Income> incomes, string? source)
    {
        return string.IsNullOrWhiteSpace(source)
            ? incomes
            : incomes.Where(i => i.Source == source);
    }

    public static IQueryable<Income> ApplyDateRangeFilter(this IQueryable<Income> incomes, DateTime? startDate, DateTime? endDate)
    {
        if (startDate is null || endDate is null)
            return incomes;

        return incomes.Where(i => i.Date >= startDate && i.Date <= endDate);
    }

    public static IQueryable<Income> ApplyAmountFilter(this IQueryable<Income> incomes, decimal? minAmount, decimal? maxAmount)
    {
        if (minAmount is null && maxAmount is null)
            return incomes;

        return incomes.Where(i => (minAmount == null || i.Amount >= minAmount) &&
                                 (maxAmount == null || i.Amount <= maxAmount));
    }

    public static IQueryable<SavingsGoal> ApplyUserFilter(this IQueryable<SavingsGoal> savingsGoals, Guid? userId)
    {
        return userId is null
            ? savingsGoals
            : savingsGoals.Where(sg => sg.UserId == userId);
    }

    public static IQueryable<SavingsGoal> ApplyGoalNameFilter(this IQueryable<SavingsGoal> savingsGoals, string? goalName)
    {
        return string.IsNullOrWhiteSpace(goalName)
            ? savingsGoals
            : savingsGoals.Where(sg => sg.GoalName == goalName);
    }

    public static IQueryable<SavingsGoal> ApplyDeadlineFilter(this IQueryable<SavingsGoal> savingsGoals, DateTime? deadline)
    {
        return deadline is null
            ? savingsGoals
            : savingsGoals.Where(sg => sg.Deadline <= deadline);
    }

    public static IQueryable<Notification> ApplyUserFilter(this IQueryable<Notification> notifications, Guid? userId)
    {
        return userId is null
            ? notifications
            : notifications.Where(n => n.UserId == userId);
    }

    public static IQueryable<Notification> ApplyDateRangeFilter(this IQueryable<Notification> notifications, DateTime? startDate, DateTime? endDate)
    {
        if (startDate is null || endDate is null)
            return notifications;

        return notifications.Where(n => n.Date >= startDate && n.Date <= endDate);
    }
}