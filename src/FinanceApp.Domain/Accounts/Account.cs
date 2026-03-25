using FinanceApp.Domain.Accounts.Events;
using FinanceApp.Domain.Accounts.ValueObjects;
using FinanceApp.Domain.Common;
using FinanceApp.Domain.Shared;

namespace FinanceApp.Domain.Accounts;

public sealed class Account : AggregateRoot
{
    public Guid CustomerId { get; private set; }
    public AccountNumber Number { get; private set; } = null!;
    public AccountType Type { get; private set; }
    public AccountStatus Status { get; private set; }
    public Money Balance { get; private set; } = null!;

    private Account() { }

    private Account(Guid id, Guid customerId, AccountNumber number, AccountType type)
        : base(id)
    {
        CustomerId = customerId;
        Number = number;
        Type = type;
        Status = AccountStatus.Active;
        Balance = Money.Zero();
    }

    public static Account Open(Guid customerId, AccountType type)
    {
        var account = new Account(
            Guid.NewGuid(),
            customerId,
            AccountNumber.Generate(),
            type);

        account.RaiseDomainEvent(new AccountOpenedEvent(account.Id, customerId, type));
        return account;
    }

    public void Credit(Money amount)
    {
        EnsureActive();
        Balance = Balance.Add(amount);
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new BalanceUpdatedEvent(Id, Balance));
    }

    public void Debit(Money amount)
    {
        EnsureActive();
        if (Balance.IsLessThan(amount))
            throw new InvalidOperationException("Insufficient balance.");

        Balance = Balance.Subtract(amount);
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new BalanceUpdatedEvent(Id, Balance));
    }

    public void Close()
    {
        if (Balance.Amount > 0)
            throw new InvalidOperationException("Cannot close account with positive balance.");

        Status = AccountStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new AccountClosedEvent(Id));
    }

    private void EnsureActive()
    {
        if (Status != AccountStatus.Active)
            throw new InvalidOperationException($"Account is not active. Current status: {Status}.");
    }
}

public enum AccountType { Checking, Savings, Investment }
public enum AccountStatus { Active, Blocked, Closed }
