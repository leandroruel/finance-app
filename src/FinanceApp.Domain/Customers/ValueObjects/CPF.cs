using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Customers.ValueObjects;

public sealed class CPF : ValueObject
{
    public string Value { get; }

    private CPF(string value) => Value = value;

    public static CPF Create(string cpf)
    {
        var digits = new string(cpf.Where(char.IsDigit).ToArray());

        if (digits.Length != 11 || digits.Distinct().Count() == 1)
            throw new ArgumentException($"'{cpf}' is not a valid CPF.", nameof(cpf));

        if (!IsValid(digits))
            throw new ArgumentException($"'{cpf}' is not a valid CPF.", nameof(cpf));

        return new CPF(digits);
    }

    private static bool IsValid(string digits)
    {
        int[] multipliers1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multipliers2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        var sum = digits[..9].Select((d, i) => (d - '0') * multipliers1[i]).Sum();
        var remainder1 = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        sum = digits[..10].Select((d, i) => (d - '0') * multipliers2[i]).Sum();
        var remainder2 = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        return digits[9] - '0' == remainder1 && digits[10] - '0' == remainder2;
    }

    public string Masked => $"{Value[..3]}.{Value[3..6]}.{Value[6..9]}-{Value[9..]}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Masked;
}
