using FluentValidation;

namespace CustomerStatement.Application.Features.Statements.Commands;

public class GenerateMonthlyStatementCommandValidator
    : AbstractValidator<GenerateMonthlyStatementCommand>
{
    public GenerateMonthlyStatementCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be greater than 0.");

        RuleFor(x => x.StatementMonth)
            .NotEmpty()
            .WithMessage("StatementMonth is required.");

        RuleFor(x => x.StatementMonth)
            .Must(x => x.Day == 1)
            .WithMessage("StatementMonth must be the first day of the month.");
    }
}