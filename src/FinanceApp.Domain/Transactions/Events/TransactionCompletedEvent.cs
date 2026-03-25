using FinanceApp.Domain.Common;
using FinanceApp.Domain.Shared;

namespace FinanceApp.Domain.Transactions.Events;

public sealed record TransactionCompletedEvent(
    Guid TransactionId,
    Guid AccountId,
    Money Amount) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
