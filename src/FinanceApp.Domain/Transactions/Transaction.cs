using FinanceApp.Domain.Common;
using FinanceApp.Domain.Shared;
using FinanceApp.Domain.Transactions.Events;

namespace FinanceApp.Domain.Transactions;

public sealed class Transaction : AggregateRoot
{
    public Guid SourceAccountId { get; private set; }
    public Guid? DestinationAccountId { get; private set; }
    public Money Amount { get; private set; } = null!;
    public TransactionType Type { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string? Description { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Transaction() { }

    private Transaction(Guid id, Guid sourceAccountId, Guid? destinationAccountId,
        Money amount, TransactionType type, string? description)
        : base(id)
    {
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        Amount = amount;
        Type = type;
        Status = TransactionStatus.Pending;
        Description = description;
    }

    public static Transaction CreateDeposit(Guid accountId, Money amount, string? description = null)
    {
        var transaction = new Transaction(Guid.NewGuid(), accountId, null, amount, TransactionType.Deposit, description);
        transaction.RaiseDomainEvent(new TransactionCreatedEvent(transaction.Id, accountId, null, amount, TransactionType.Deposit));
        return transaction;
    }

    public static Transaction CreateWithdrawal(Guid accountId, Money amount, string? description = null)
    {
        var transaction = new Transaction(Guid.NewGuid(), accountId, null, amount, TransactionType.Withdrawal, description);
        transaction.RaiseDomainEvent(new TransactionCreatedEvent(transaction.Id, accountId, null, amount, TransactionType.Withdrawal));
        return transaction;
    }

    public static Transaction CreateTransfer(Guid sourceAccountId, Guid destinationAccountId, Money amount, string? description = null)
    {
        var transaction = new Transaction(Guid.NewGuid(), sourceAccountId, destinationAccountId, amount, TransactionType.Transfer, description);
        transaction.RaiseDomainEvent(new TransactionCreatedEvent(transaction.Id, sourceAccountId, destinationAccountId, amount, TransactionType.Transfer));
        return transaction;
    }

    public void Complete()
    {
        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException($"Cannot complete transaction in status {Status}.");

        Status = TransactionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new TransactionCompletedEvent(Id, SourceAccountId, Amount));
    }

    public void Fail(string reason)
    {
        Status = TransactionStatus.Failed;
        Description = reason;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new TransactionFailedEvent(Id, reason));
    }
}

public enum TransactionType { Deposit, Withdrawal, Transfer, Payment }
public enum TransactionStatus { Pending, Completed, Failed, Reversed }
