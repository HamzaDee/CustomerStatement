using CustomerStatement.Application.DTOs;
using MediatR;

namespace CustomerStatement.Application.Features.Statements.Queries;

public record GetStatementsQuery(
    int CustomerId,
    DateTime? FromMonth,
    DateTime? ToMonth
) : IRequest<List<AccountStatementDto>>;