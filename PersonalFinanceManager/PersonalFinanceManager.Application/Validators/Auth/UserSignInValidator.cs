using FluentValidation;
using PersonalFinanceManager.Application.Features.Users.Commands.SignIn;

namespace PersonalFinanceManager.Application.Validators.Auth;

/// <summary>
/// Validator class for validating user sign in requests
/// </summary>
public class UserSignInValidator : AbstractValidator<UserSignInCommand>
{
    /// <summary>
    /// Initializes a new instance of the UserSignInValidator
    /// class and defines validation rules for UserName, password and Email
    /// </summary>
    public UserSignInValidator()
    {
        RuleFor(u => u.UserSignInRequest.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress();

        RuleFor(u => u.UserSignInRequest.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty().WithMessage("Password is required")
            .Matches(@"^(?=.*[!?\*._^@$])(?=.*\d)(?=.*[A-Z])(?=.*[^\w\s]).{6,}$").WithMessage("Your password must contain at least one (!? *. _ ^ @ $)., one digit, one uppercase");
    }
}