using Wrecept.Domain;

namespace Wrecept.Domain.Tests.ValueObjects;

public class TaxRateTests
{
    [Fact]
    public void Constructor_InvalidRate_Throws()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new TaxRate(1.5m));
    }

    [Fact]
    public void ApplyTo_CalculatesTaxAmount()
    {
        // Arrange
        var rate = new TaxRate(0.27m);
        var net = new Money("HUF", 100m);

        // Act
        var tax = rate.ApplyTo(net);

        // Assert
        Assert.Equal(27m, tax.Amount);
        Assert.Equal("HUF", tax.Currency);
    }
}
