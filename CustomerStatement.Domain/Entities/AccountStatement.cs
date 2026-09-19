namespace CustomerStatement.Domain.Entities;

public class AccountStatement
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime StatementMonth { get; set; }

    public decimal OpeningBalance { get; set; }

    public decimal ClosingBalance { get; set; }

    public DateTime GeneratedAt { get; set; }

    public Customer Customer { get; set; } = null!;

    public ICollection<StatementTransaction> Transactions { get; set; } = new List<StatementTransaction>();
}