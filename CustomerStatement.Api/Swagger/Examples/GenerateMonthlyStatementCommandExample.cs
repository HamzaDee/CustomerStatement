using CustomerStatement.Application.Features.Statements.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace CustomerStatement.Api.Swagger.Examples;

public class GenerateMonthlyStatementCommandExample
    : IExamplesProvider<GenerateMonthlyStatementCommand>
{
    public GenerateMonthlyStatementCommand GetExamples()
    {
        return new GenerateMonthlyStatementCommand(
            1,
            new DateTime(2026, 6, 1));
    }
}