using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Accounts.ValueObjects;

public sealed class AccountNumber : ValueObject
{
    public string Value { get; }

    private AccountNumber(string value) => Value = value;

    public static AccountNumber Generate()
    {
        var number = Random.Shared.NextInt64(1_000_000_000L, 9_999_999_999L).ToString();
        return new AccountNumber(number);
    }

    public static AccountNumber From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 10)
            throw new ArgumentException("Invalid account number.", nameof(value));
        return new AccountNumber(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"{Value[..5]}-{Value[5..]}";
}
