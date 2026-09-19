using FluentValidation;

namespace CustomerStatement.Application.Features.Statements.Queries;

public class GetStatementsQueryValidator
    : AbstractValidator<GetStatementsQuery>
{
    public GetStatementsQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be greater than 0.");

        RuleFor(x => x.FromMonth)
            .Must(x => !x.HasValue || x.Value.Day == 1)
            .WithMessage("FromMonth must be the first day of the month.");

        RuleFor(x => x.ToMonth)
            .Must(x => !x.HasValue || x.Value.Day == 1)
            .WithMessage("ToMonth must be the first day of the month.");

        RuleFor(x => x)
            .Must(x =>
                !x.FromMonth.HasValue ||
                !x.ToMonth.HasValue ||
                x.FromMonth.Value <= x.ToMonth.Value)
            .WithMessage(
                "FromMonth must be less than or equal to ToMonth.");
    }
}