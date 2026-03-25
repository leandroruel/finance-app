using FluentValidation;

namespace FinanceApp.Application.Customers.Commands.RegisterCustomer;

public sealed class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF is required.")
            .Matches(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$").WithMessage("CPF must be in a valid format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$").WithMessage("Phone number must be in a valid Brazilian format.");
    }
}
