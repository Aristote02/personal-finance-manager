using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Domain.Entities;

namespace PersonalFinanceManager.Infrastructure.Repositories.Extensions;

public static class SearchExtensions
{
    public static IQueryable<Budget> SearchBudgets(this IQueryable<Budget> budgets, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return budgets;

        var searchPattern = $"%{search.Trim().ToLower()}%";
        return budgets.Where(b => EF.Functions.Like(b.Category.ToLower(), searchPattern));
    }

    public static IQueryable<Expense> SearchExpenses(this IQueryable<Expense> expenses, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return expenses;

        var searchPattern = $"%{search.Trim().ToLower()}%";
        return expenses.Where(e => EF.Functions.Like(e.Description.ToLower(), searchPattern)
                                  || EF.Functions.Like(e.Category.ToLower(), searchPattern));
    }

    public static IQueryable<Income> SearchIncomes(this IQueryable<Income> incomes, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return incomes;

        var searchPattern = $"%{search.Trim().ToLower()}%";
        return incomes.Where(i => EF.Functions.Like(i.Source.ToLower(), searchPattern));
    }

    public static IQueryable<Notification> SearchNotifications(this IQueryable<Notification> notifications, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return notifications;

        var searchPattern = $"%{search.Trim().ToLower()}%";
        return notifications.Where(n => EF.Functions.Like(n.Message.ToLower(), searchPattern));
    }

    public static IQueryable<SavingsGoal> SearchSavingsGoals(this IQueryable<SavingsGoal> savingsGoals, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return savingsGoals;

        var searchPattern = $"%{search.Trim().ToLower()}%";
        return savingsGoals.Where(sg => EF.Functions.Like(sg.GoalName.ToLower(), searchPattern));
    }
}