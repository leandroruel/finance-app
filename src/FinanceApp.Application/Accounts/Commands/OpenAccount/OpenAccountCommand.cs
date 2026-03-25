using ErrorOr;
using FinanceApp.Domain.Accounts;
using MediatR;

namespace FinanceApp.Application.Accounts.Commands.OpenAccount;

public sealed record OpenAccountCommand(
    Guid CustomerId,
    AccountType Type) : IRequest<ErrorOr<OpenAccountResult>>;

public sealed record OpenAccountResult(Guid AccountId, string AccountNumber, AccountType Type);
