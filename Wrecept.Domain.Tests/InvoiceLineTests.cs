using Wrecept.Domain;

namespace Wrecept.Domain.Tests;

public class InvoiceLineTests
{
    private static Product SampleProduct() => new(Guid.NewGuid(), "Prod", new Money("USD", 1m), new TaxRate(Guid.NewGuid(), "T", 0.2m));

    [Fact]
    public void Constructor_NullProduct_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new InvoiceLine(null!, 1, new Money("USD", 1m)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_NonPositiveQuantity_Throws(int qty)
    {
        var product = SampleProduct();
        var price = new Money("USD", 1m);
        Assert.Throws<ArgumentOutOfRangeException>(() => new InvoiceLine(product, qty, price));
    }

    [Fact]
    public void Constructor_NullUnitPrice_Throws()
    {
        var product = SampleProduct();
        Assert.Throws<ArgumentNullException>(() => new InvoiceLine(product, 1, null!));
    }

    [Fact]
    public void Constructor_NonPositiveUnitPrice_Throws()
    {
        var product = SampleProduct();
        var price = new Money("USD", 0m);
        Assert.Throws<ArgumentException>(() => new InvoiceLine(product, 1, price));
    }

    [Fact]
    public void Total_ReturnsQuantityTimesPrice()
    {
        var product = SampleProduct();
        var price = new Money("USD", 2m);
        var line = new InvoiceLine(product, 3, price);
        Assert.Equal(6m, line.Total.Amount);
        Assert.Equal("USD", line.Total.Currency);
    }
}
