using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Customers.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber Create(string phone)
    {
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        if (digits.Length is not (10 or 11))
            throw new ArgumentException($"'{phone}' is not a valid Brazilian phone number.", nameof(phone));

        return new PhoneNumber(digits);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.Length == 11
        ? $"({Value[..2]}) {Value[2]}{Value[3..7]}-{Value[7..]}"
        : $"({Value[..2]}) {Value[2..6]}-{Value[6..]}";
}
