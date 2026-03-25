using Asp.Versioning;
using ErrorOr;
using FinanceApp.Application.Customers.Commands.RegisterCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/customers")]
[Authorize]
public sealed class CustomersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterCustomerResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCustomerCommand(request.Name, request.Email, request.CPF, request.PhoneNumber);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            created => CreatedAtAction(nameof(GetById), new { id = created.CustomerId }, created),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        // TODO: implement GetCustomerQuery
        return Ok();
    }

    private IActionResult Problem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelStateDictionary = errors
                .GroupBy(e => e.Code)
                .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

            return ValidationProblem(new ValidationProblemDetails(modelStateDictionary));
        }

        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}

public sealed record RegisterCustomerRequest(string Name, string Email, string CPF, string PhoneNumber);
