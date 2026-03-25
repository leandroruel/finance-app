using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Accounts.Events;

public sealed record AccountClosedEvent(Guid AccountId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
