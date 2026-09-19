using CustomerStatement.Application.Interfaces;
using CustomerStatement.Domain.Entities;
using CustomerStatement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerStatement.Infrastructure.Repositories;

public class StatementRepository : IStatementRepository
{
    private readonly ApplicationDbContext _context;

    public StatementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AccountStatement>> GetStatementsAsync(
    int customerId,
    DateTime? fromMonth,
    DateTime? toMonth,
    CancellationToken cancellationToken)
    {
        var query = _context.AccountStatements
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Transactions)
            .Where(x => x.CustomerId == customerId);

        if (fromMonth.HasValue)
        {
            query = query.Where(
                x => x.StatementMonth >= fromMonth.Value);
        }

        if (toMonth.HasValue)
        {
            query = query.Where(
                x => x.StatementMonth <= toMonth.Value);
        }

        return await query
            .OrderBy(x => x.StatementMonth)
            .ToListAsync(cancellationToken);
    }
}