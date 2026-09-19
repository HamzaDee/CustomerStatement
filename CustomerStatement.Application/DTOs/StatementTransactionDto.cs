namespace CustomerStatement.Application.DTOs;

public class StatementTransactionDto
{
    public int Id { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }
}