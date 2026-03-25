using ErrorOr;
using FinanceApp.Application.Common.Interfaces;
using FinanceApp.Domain.Customers;
using MediatR;

namespace FinanceApp.Application.Customers.Commands.RegisterCustomer;

public sealed class RegisterCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterCustomerCommand, ErrorOr<RegisterCustomerResult>>
{
    public async Task<ErrorOr<RegisterCustomerResult>> Handle(
        RegisterCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var emailExists = await customerRepository.ExistsByEmailAsync(command.Email, cancellationToken);
        if (emailExists)
            return Error.Conflict("Customer.EmailConflict", $"Email '{command.Email}' is already in use.");

        var customer = Customer.Create(command.Name, command.Email, command.CPF, command.PhoneNumber);

        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterCustomerResult(customer.Id, customer.Name, customer.Email.Value);
    }
}
