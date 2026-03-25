using MediatR;

namespace FinanceApp.Domain.Common;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
