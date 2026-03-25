using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Accounts.Events;

public sealed record AccountOpenedEvent(
    Guid AccountId,
    Guid CustomerId,
    AccountType Type) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
