using Wrecept.Domain;

namespace Wrecept.Domain.Tests;

public class ProductTests
{
    private static TaxRate StandardTax => new(Guid.NewGuid(), "Standard", 0.2m);

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        var price = new Money("USD", 1m);
        Assert.Throws<ArgumentException>(() => new Product(Guid.Empty, "Name", price, StandardTax));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyName_Throws(string name)
    {
        var price = new Money("USD", 1m);
        Assert.Throws<ArgumentException>(() => new Product(Guid.NewGuid(), name, price, StandardTax));
    }

    [Fact]
    public void Constructor_NullPrice_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Product(Guid.NewGuid(), "Name", null!, StandardTax));
    }

    [Fact]
    public void Constructor_NonPositivePrice_Throws()
    {
        var price = new Money("USD", 0m);
        Assert.Throws<ArgumentException>(() => new Product(Guid.NewGuid(), "Name", price, StandardTax));
    }

    [Fact]
    public void Constructor_NullTaxRate_Throws()
    {
        var price = new Money("USD", 1m);
        Assert.Throws<ArgumentNullException>(() => new Product(Guid.NewGuid(), "Name", price, null!));
    }

    [Fact]
    public void Constructor_Valid_SetsProperties()
    {
        var id = Guid.NewGuid();
        var price = new Money("USD", 1m);
        var tax = StandardTax;
        var product = new Product(id, "Bread", price, tax);
        Assert.Equal(id, product.Id);
        Assert.Equal("Bread", product.Name);
        Assert.Equal(price, product.Price);
        Assert.Equal(tax, product.TaxRate);
    }
}
