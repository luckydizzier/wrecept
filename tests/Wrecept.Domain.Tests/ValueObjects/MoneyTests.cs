using Wrecept.Domain;

namespace Wrecept.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_Sets_Properties()
    {
        // Arrange & Act
        var money = new Money("HUF", 10m);

        // Assert
        Assert.Equal("HUF", money.Currency);
        Assert.Equal(10m, money.Amount);
    }

    [Fact]
    public void Add_SameCurrency_ReturnsSum()
    {
        // Arrange
        var first = new Money("HUF", 10m);
        var second = new Money("HUF", 5m);

        // Act
        var result = first.Add(second);

        // Assert
        Assert.Equal(15m, result.Amount);
        Assert.Equal("HUF", result.Currency);
    }

    [Fact]
    public void Add_DifferentCurrency_Throws()
    {
        // Arrange
        var first = new Money("HUF", 10m);
        var second = new Money("USD", 5m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => first.Add(second));
    }

    [Fact]
    public void Multiply_Negative_Throws()
    {
        // Arrange
        var money = new Money("HUF", 10m);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => money.Multiply(-1));
    }
}
