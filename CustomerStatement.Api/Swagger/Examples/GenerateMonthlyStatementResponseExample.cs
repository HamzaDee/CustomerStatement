using CustomerStatement.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace CustomerStatement.Api.Swagger.Examples;

public class GenerateMonthlyStatementResponseExample
    : IExamplesProvider<GenerateMonthlyStatementResponseDto>
{
    public GenerateMonthlyStatementResponseDto GetExamples()
    {
        return new GenerateMonthlyStatementResponseDto
        {
            StatementId = 6,
            Message = "Monthly statement generated successfully."
        };
    }
}