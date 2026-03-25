using FinanceApp.Domain.Common;
using FinanceApp.Domain.Shared;

namespace FinanceApp.Domain.Transactions.Events;

public sealed record TransactionCreatedEvent(
    Guid TransactionId,
    Guid SourceAccountId,
    Guid? DestinationAccountId,
    Money Amount,
    TransactionType Type) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
