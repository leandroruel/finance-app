using ErrorOr;
using MediatR;

namespace FinanceApp.Application.Customers.Commands.RegisterCustomer;

public sealed record RegisterCustomerCommand(
    string Name,
    string Email,
    string CPF,
    string PhoneNumber) : IRequest<ErrorOr<RegisterCustomerResult>>;

public sealed record RegisterCustomerResult(Guid CustomerId, string Name, string Email);
