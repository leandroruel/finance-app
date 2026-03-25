using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Cards.Events;

public sealed record CardBlockedEvent(Guid CardId, string Reason) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
