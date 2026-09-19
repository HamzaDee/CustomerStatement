using CustomerStatement.Application.DTOs;
using CustomerStatement.Application.Interfaces;
using MediatR;

namespace CustomerStatement.Application.Features.Statements.Queries;

public class GetStatementsQueryHandler
    : IRequestHandler<GetStatementsQuery, List<AccountStatementDto>>
{
    private readonly IStatementRepository _statementRepository;

    public GetStatementsQueryHandler(
        IStatementRepository statementRepository)
    {
        _statementRepository = statementRepository;
    }

    public async Task<List<AccountStatementDto>> Handle(
        GetStatementsQuery request,
        CancellationToken cancellationToken)
    {
        var statements = await _statementRepository.GetStatementsAsync(
            request.CustomerId,
            request.FromMonth,
            request.ToMonth,
            cancellationToken);

        return statements.Select(statement => new AccountStatementDto
        {
            Id = statement.Id,
            CustomerId = statement.CustomerId,
            CustomerName = statement.Customer.Name,
            CustomerEmail = statement.Customer.Email,
            StatementMonth = statement.StatementMonth,
            OpeningBalance = statement.OpeningBalance,
            ClosingBalance = statement.ClosingBalance,
            GeneratedAt = statement.GeneratedAt,

            Transactions = statement.Transactions
                .Select(transaction => new StatementTransactionDto
                {
                    Id = transaction.Id,
                    TransactionDate = transaction.TransactionDate,
                    Description = transaction.Description,
                    Debit = transaction.Debit,
                    Credit = transaction.Credit
                })
                .ToList()
        }).ToList();
    }
}