using CustomerStatement.Domain.Entities;

namespace CustomerStatement.Application.Interfaces;

public interface IStatementRepository
{
    Task<List<AccountStatement>> GetStatementsAsync(
        int customerId,
        DateTime? fromMonth,
        DateTime? toMonth,
        CancellationToken cancellationToken);
}