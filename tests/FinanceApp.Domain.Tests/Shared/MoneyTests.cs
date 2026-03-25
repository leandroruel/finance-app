using FinanceApp.Domain.Shared;
using FluentAssertions;

namespace FinanceApp.Domain.Tests.Shared;

public class MoneyTests
{
    [Fact]
    public void Of_WithValidAmount_ShouldCreateMoney()
    {
        var money = Money.Of(100.50m, "BRL");
        money.Amount.Should().Be(100.50m);
        money.Currency.Should().Be("BRL");
    }

    [Fact]
    public void Of_WithNegativeAmount_ShouldThrow()
    {
        var act = () => Money.Of(-1m);
        act.Should().Throw<ArgumentException>().WithMessage("*negative*");
    }

    [Fact]
    public void Add_SameCurrency_ShouldSumAmounts()
    {
        var a = Money.Of(100m);
        var b = Money.Of(50m);
        a.Add(b).Amount.Should().Be(150m);
    }

    [Fact]
    public void Add_DifferentCurrencies_ShouldThrow()
    {
        var brl = Money.Of(100m, "BRL");
        var usd = Money.Of(100m, "USD");
        var act = () => brl.Add(usd);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Subtract_ShouldResultInCorrectAmount()
    {
        var a = Money.Of(200m);
        var b = Money.Of(75m);
        a.Subtract(b).Amount.Should().Be(125m);
    }

    [Fact]
    public void Equality_SameAmountAndCurrency_ShouldBeEqual()
    {
        var a = Money.Of(100m, "BRL");
        var b = Money.Of(100m, "BRL");
        a.Should().Be(b);
    }
}
