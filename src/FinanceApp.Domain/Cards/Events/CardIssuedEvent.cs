using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Cards.Events;

public sealed record CardIssuedEvent(Guid CardId, Guid AccountId, CardType Type) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
