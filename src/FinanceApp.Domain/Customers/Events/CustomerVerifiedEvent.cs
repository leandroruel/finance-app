using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Customers.Events;

public sealed record CustomerVerifiedEvent(Guid CustomerId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
