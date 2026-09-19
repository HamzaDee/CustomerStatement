using CustomerStatement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace CustomerStatement.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            """
            ==================================================
            EMAIL SENT
            ==================================================
            To: {To}
            Subject: {Subject}

            Body:
            {Body}
            ==================================================
            """,
            to,
            subject,
            body);

        return Task.CompletedTask;
    }
}