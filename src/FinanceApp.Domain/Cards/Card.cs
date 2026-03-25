using FinanceApp.Domain.Cards.Events;
using FinanceApp.Domain.Cards.ValueObjects;
using FinanceApp.Domain.Common;
using FinanceApp.Domain.Shared;

namespace FinanceApp.Domain.Cards;

public sealed class Card : AggregateRoot
{
    public Guid AccountId { get; private set; }
    public MaskedCardNumber Number { get; private set; } = null!;
    public CardType Type { get; private set; }
    public CardStatus Status { get; private set; }
    public Money? CreditLimit { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private Card() { }

    private Card(Guid id, Guid accountId, MaskedCardNumber number, CardType type, Money? creditLimit)
        : base(id)
    {
        AccountId = accountId;
        Number = number;
        Type = type;
        Status = CardStatus.Active;
        CreditLimit = creditLimit;
        ExpiresAt = DateTime.UtcNow.AddYears(4);
    }

    public static Card Issue(Guid accountId, CardType type, Money? creditLimit = null)
    {
        if (type == CardType.Credit && creditLimit is null)
            throw new ArgumentException("Credit limit is required for credit cards.", nameof(creditLimit));

        var card = new Card(Guid.NewGuid(), accountId, MaskedCardNumber.Generate(), type, creditLimit);
        card.RaiseDomainEvent(new CardIssuedEvent(card.Id, accountId, type));
        return card;
    }

    public void Block(string reason)
    {
        if (Status == CardStatus.Blocked)
            throw new InvalidOperationException("Card is already blocked.");

        Status = CardStatus.Blocked;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new CardBlockedEvent(Id, reason));
    }

    public void Unblock()
    {
        if (Status != CardStatus.Blocked)
            throw new InvalidOperationException("Card is not blocked.");

        Status = CardStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCreditLimit(Money newLimit)
    {
        if (Type != CardType.Credit)
            throw new InvalidOperationException("Cannot set credit limit on a debit card.");

        CreditLimit = newLimit;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new CardLimitUpdatedEvent(Id, newLimit));
    }
}

public enum CardType { Debit, Credit }
public enum CardStatus { Active, Blocked, Cancelled, Expired }
