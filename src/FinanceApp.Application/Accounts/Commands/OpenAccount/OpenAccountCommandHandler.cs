using ErrorOr;
using FinanceApp.Application.Common.Interfaces;
using FinanceApp.Domain.Accounts;
using MediatR;

namespace FinanceApp.Application.Accounts.Commands.OpenAccount;

public sealed class OpenAccountCommandHandler(
    ICustomerRepository customerRepository,
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<OpenAccountCommand, ErrorOr<OpenAccountResult>>
{
    public async Task<ErrorOr<OpenAccountResult>> Handle(
        OpenAccountCommand command,
        CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer is null)
            return Error.NotFound("Customer.NotFound", $"Customer '{command.CustomerId}' not found.");

        if (customer.Status != Domain.Customers.CustomerStatus.Verified)
            return Error.Validation("Customer.NotVerified", "Customer must be verified before opening an account.");

        var account = Account.Open(command.CustomerId, command.Type);

        await accountRepository.AddAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new OpenAccountResult(account.Id, account.Number.ToString(), account.Type);
    }
}
