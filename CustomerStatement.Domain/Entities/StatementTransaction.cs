namespace CustomerStatement.Domain.Entities;

public class StatementTransaction
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int? AccountStatementId { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public Customer Customer { get; set; } = null!;

    public AccountStatement? AccountStatement { get; set; }
}