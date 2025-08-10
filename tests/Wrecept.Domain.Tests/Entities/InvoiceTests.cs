using Wrecept.Domain;

namespace Wrecept.Domain.Tests.Entities;

public class InvoiceTests
{
    private static InvoiceLine CreateLine(decimal amount = 100m, string currency = "HUF")
    {
        var product = new Product(Guid.NewGuid(), "Item", new Money(currency, amount), new TaxRate(0.27m));
        return new InvoiceLine(product, 1, new Money(currency, amount));
    }

    [Fact]
    public void Constructor_Sets_Total()
    {
        // Arrange
        var line1 = CreateLine(100m);
        var line2 = CreateLine(50m);
        var customer = new Customer(Guid.NewGuid(), "Alice");

        // Act
        var invoice = new Invoice(Guid.NewGuid(), customer, new[] { line1, line2 });

        // Assert
        Assert.Equal(150m, invoice.Total.Amount);
        Assert.Equal("HUF", invoice.Total.Currency);
    }

    [Fact]
    public void Constructor_EmptyLines_Throws()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "Alice");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Invoice(Guid.NewGuid(), customer, Array.Empty<InvoiceLine>()));
    }

    [Fact]
    public void Constructor_MixedCurrencies_Throws()
    {
        // Arrange
        var line1 = CreateLine(10m, "HUF");
        var line2 = CreateLine(5m, "USD");
        var customer = new Customer(Guid.NewGuid(), "Alice");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new Invoice(Guid.NewGuid(), customer, new[] { line1, line2 }));
    }

    [Fact]
    public void InvoiceLine_InvalidQuantity_Throws()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Item", new Money("HUF", 10m), new TaxRate(0.27m));
        var unitPrice = new Money("HUF", 10m);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new InvoiceLine(product, 0, unitPrice));
    }
}
