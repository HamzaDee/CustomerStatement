namespace CustomerStatement.Application.DTOs;

public class AccountStatementDto
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public DateTime StatementMonth { get; set; }

    public decimal OpeningBalance { get; set; }

    public decimal ClosingBalance { get; set; }

    public DateTime GeneratedAt { get; set; }

    public List<StatementTransactionDto> Transactions { get; set; }
        = new();
}