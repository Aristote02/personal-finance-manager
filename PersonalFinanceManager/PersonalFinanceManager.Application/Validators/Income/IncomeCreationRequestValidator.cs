using FluentValidation;
using PersonalFinanceManager.Shared.Requests.Incomes;

namespace PersonalFinanceManager.Application.Validators.Income;

public class IncomeCreationRequestValidator : AbstractValidator<IncomeCreationRequest>
{
    public IncomeCreationRequestValidator()
    {
        RuleFor(income => income.Source)
            .NotEmpty().WithMessage("Income source is required")
            .MinimumLength(3).WithMessage("Income source must be at least 3 characters long");

        RuleFor(Income => Income.Amount)
            .GreaterThan(0).WithMessage("Income amount must be greater than 0");

        RuleFor(income => income.Date)
            .NotEmpty().WithMessage("Income date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Income date cannot be a future date");

        RuleFor(income => income.UserId)
            .NotEqual(Guid.Empty).WithMessage("User Id is required");
    }
}