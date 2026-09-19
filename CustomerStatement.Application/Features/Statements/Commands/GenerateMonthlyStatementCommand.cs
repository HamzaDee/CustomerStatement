using MediatR;

namespace CustomerStatement.Application.Features.Statements.Commands;

public record GenerateMonthlyStatementCommand(
    int CustomerId,
    DateTime StatementMonth
) : IRequest<int>;