using CustomerStatement.Api.Swagger.Examples;
using CustomerStatement.Application.DTOs;
using CustomerStatement.Application.Features.Statements.Commands;
using CustomerStatement.Application.Features.Statements.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace CustomerStatement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK,typeof(GetStatementsResponseExample))]
    [ProducesResponseType(typeof(List<AccountStatementDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<AccountStatementDto>>> GetStatements([FromQuery] int customerId,[FromQuery] DateTime? fromMonth,[FromQuery] DateTime? toMonth,CancellationToken cancellationToken)
    {
        var query = new GetStatementsQuery(customerId,fromMonth,toMonth);

        var result = await _mediator.Send(query,cancellationToken);

        return Ok(result);
    }

    [HttpPost("generate")]
    [SwaggerRequestExample(typeof(GenerateMonthlyStatementCommand),typeof(GenerateMonthlyStatementCommandExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK,typeof(GenerateMonthlyStatementResponseExample))]
    [ProducesResponseType(typeof(GenerateMonthlyStatementResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GenerateMonthlyStatementResponseDto>>
        GenerateMonthlyStatement([FromBody] GenerateMonthlyStatementCommand command,CancellationToken cancellationToken)
    {
        var statementId = await _mediator.Send(command,cancellationToken);
        return Ok(new GenerateMonthlyStatementResponseDto
        {
            StatementId = statementId,
            Message = "Monthly statement generated successfully."
        });
    }
}