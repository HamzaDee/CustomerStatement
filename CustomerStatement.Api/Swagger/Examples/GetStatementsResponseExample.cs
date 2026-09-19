using CustomerStatement.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace CustomerStatement.Api.Swagger.Examples;

public class GetStatementsResponseExample
    : IExamplesProvider<List<AccountStatementDto>>
{
    public List<AccountStatementDto> GetExamples()
    {
        return new List<AccountStatementDto>
        {
            new AccountStatementDto
            {
                Id = 1,
                CustomerId = 1,
                CustomerName = "John Smith",
                CustomerEmail = "john.smith@example.com",
                StatementMonth = new DateTime(2026, 1, 1),
                OpeningBalance = 1000m,
                ClosingBalance = 1250m,
                GeneratedAt = new DateTime(2026, 1, 31, 10, 0, 0),
                Transactions = new List<StatementTransactionDto>
                {
                    new StatementTransactionDto
                    {
                        Id = 1,
                        TransactionDate = new DateTime(2026, 1, 5),
                        Description = "Salary",
                        Debit = 0m,
                        Credit = 500m
                    },
                    new StatementTransactionDto
                    {
                        Id = 2,
                        TransactionDate = new DateTime(2026, 1, 10),
                        Description = "Online Purchase",
                        Debit = 100m,
                        Credit = 0m
                    },
                    new StatementTransactionDto
                    {
                        Id = 3,
                        TransactionDate = new DateTime(2026, 1, 20),
                        Description = "Bank Transfer",
                        Debit = 150m,
                        Credit = 0m
                    }
                }
            }
        };
    }
}