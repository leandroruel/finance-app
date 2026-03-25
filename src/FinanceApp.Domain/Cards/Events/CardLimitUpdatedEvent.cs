using FinanceApp.Domain.Common;
using FinanceApp.Domain.Shared;

namespace FinanceApp.Domain.Cards.Events;

public sealed record CardLimitUpdatedEvent(Guid CardId, Money NewLimit) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
