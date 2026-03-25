using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Cards.ValueObjects;

public sealed class MaskedCardNumber : ValueObject
{
    public string LastFour { get; }
    public string Masked => $"**** **** **** {LastFour}";

    private MaskedCardNumber(string lastFour) => LastFour = lastFour;

    public static MaskedCardNumber Generate()
    {
        var lastFour = Random.Shared.Next(1000, 9999).ToString();
        return new MaskedCardNumber(lastFour);
    }

    public static MaskedCardNumber From(string lastFour)
    {
        if (lastFour.Length != 4 || !lastFour.All(char.IsDigit))
            throw new ArgumentException("Last four must be 4 digits.", nameof(lastFour));
        return new MaskedCardNumber(lastFour);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return LastFour;
    }

    public override string ToString() => Masked;
}
