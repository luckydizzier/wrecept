using Wrecept.Domain;

namespace Wrecept.Domain.Tests;

public class MoneyTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyCurrency_Throws(string currency)
    {
        Assert.Throws<ArgumentException>(() => new Money(currency!, 1m));
    }

    [Fact]
    public void Constructor_NegativeAmount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Money("USD", -1m));
    }

    [Fact]
    public void Add_SameCurrency_Sums()
    {
        var first = new Money("USD", 5m);
        var second = new Money("USD", 7m);

        var result = first.Add(second);

        Assert.Equal(12m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Add_DifferentCurrency_Throws()
    {
        var first = new Money("USD", 5m);
        var second = new Money("EUR", 3m);

        Assert.Throws<InvalidOperationException>(() => first.Add(second));
    }

    [Fact]
    public void Add_Null_Throws()
    {
        var money = new Money("USD", 5m);
        Assert.Throws<ArgumentNullException>(() => money.Add(null!));
    }

    [Fact]
    public void Multiply_ByFactor_ReturnsProduct()
    {
        var money = new Money("USD", 5m);
        var result = money.Multiply(3);
        Assert.Equal(15m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Multiply_NegativeFactor_Throws()
    {
        var money = new Money("USD", 5m);
        Assert.Throws<ArgumentOutOfRangeException>(() => money.Multiply(-1));
    }
}
