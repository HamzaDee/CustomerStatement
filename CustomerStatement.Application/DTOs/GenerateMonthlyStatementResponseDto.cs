namespace CustomerStatement.Application.DTOs;

public class GenerateMonthlyStatementResponseDto
{
    public int StatementId { get; set; }

    public string Message { get; set; } = string.Empty;
}