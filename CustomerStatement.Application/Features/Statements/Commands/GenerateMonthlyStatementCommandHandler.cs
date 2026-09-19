using CustomerStatement.Application.Interfaces;
using CustomerStatement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerStatement.Application.Features.Statements.Commands;

public class GenerateMonthlyStatementCommandHandler
    : IRequestHandler<GenerateMonthlyStatementCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    public GenerateMonthlyStatementCommandHandler(IApplicationDbContext context , IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<int> Handle(GenerateMonthlyStatementCommand request,CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(
                x => x.Id == request.CustomerId,
                cancellationToken);

        if (customer == null)
            throw new KeyNotFoundException("Customer not found.");

        var statementMonth = new DateTime(
            request.StatementMonth.Year,
            request.StatementMonth.Month,
            1);

        var existingStatement = await _context.AccountStatements
            .FirstOrDefaultAsync(
                x => x.CustomerId == request.CustomerId &&
                     x.StatementMonth == statementMonth,
                cancellationToken);

        if (existingStatement != null)
            return existingStatement.Id;

        var previousStatement = await _context.AccountStatements
            .Where(x =>
                x.CustomerId == request.CustomerId &&
                x.StatementMonth < statementMonth)
            .OrderByDescending(x => x.StatementMonth)
            .FirstOrDefaultAsync(cancellationToken);

        var openingBalance = previousStatement?.ClosingBalance ?? 0m;

        var nextMonth = statementMonth.AddMonths(1);

        var transactions = await _context.StatementTransactions
            .Where(x =>
                x.CustomerId == request.CustomerId &&
                x.TransactionDate >= statementMonth &&
                x.TransactionDate < nextMonth)
            .ToListAsync(cancellationToken);

        var closingBalance = openingBalance
            + transactions.Sum(x => x.Credit)
            - transactions.Sum(x => x.Debit);

        var statement = new AccountStatement
        {
            CustomerId = request.CustomerId,
            StatementMonth = statementMonth,
            OpeningBalance = openingBalance,
            ClosingBalance = closingBalance,
            GeneratedAt = DateTime.UtcNow
        };

        _context.AccountStatements.Add(statement);

        foreach (var transaction in transactions)
        {
            transaction.AccountStatement = statement;
        }

        await _context.SaveChangesAsync(cancellationToken);

        var transactionLines = transactions.Any()
         ? string.Join(
             Environment.NewLine,
             transactions.Select(t =>
                 $"{t.TransactionDate:dd/MM/yyyy} | " +
                 $"{t.Description,-20} | " +
                 $"{t.Debit,10:N2} | " +
                 $"{t.Credit,10:N2}"))
         : "No transactions for this month.";


        var emailSubject =$"Monthly Account Statement - {statementMonth:MMMM yyyy}";

        var emailBody = $"""
            Dear {customer.Name},

            Your monthly account statement is ready.

            ==================================================
                            ACCOUNT STATEMENT
            ==================================================

            Statement Month : {statementMonth:MMMM yyyy}
            Customer        : {customer.Name}
            Email           : {customer.Email}

            Opening Balance : {statement.OpeningBalance:N2}

            --------------------------------------------------
            Date         Description             Debit     Credit
            --------------------------------------------------
            {transactionLines}
            --------------------------------------------------

            Closing Balance : {statement.ClosingBalance:N2}

            ==================================================

            Thank you.
            """;       
        await _emailService.SendAsync(customer.Email,emailSubject,emailBody,cancellationToken);


        return statement.Id;
    }
}