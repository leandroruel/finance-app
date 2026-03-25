using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Customers.Events;

public sealed record CustomerRegisteredEvent(
    Guid CustomerId,
    string Name,
    string Email) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
