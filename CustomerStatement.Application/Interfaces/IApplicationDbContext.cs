using CustomerStatement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerStatement.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }

    DbSet<AccountStatement> AccountStatements { get; }

    DbSet<StatementTransaction> StatementTransactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}