using Asp.Versioning;
using ErrorOr;
using FinanceApp.Application.Accounts.Commands.OpenAccount;
using FinanceApp.Domain.Accounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/accounts")]
[Authorize]
public sealed class AccountsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(OpenAccountResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Open(
        [FromBody] OpenAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new OpenAccountCommand(request.CustomerId, request.Type);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            created => CreatedAtAction(nameof(GetById), new { id = created.AccountId }, created),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        // TODO: implement GetAccountQuery
        return Ok();
    }

    private IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}

public sealed record OpenAccountRequest(Guid CustomerId, AccountType Type);
