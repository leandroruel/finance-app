using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Transactions.Events;

public sealed record TransactionFailedEvent(Guid TransactionId, string Reason) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
