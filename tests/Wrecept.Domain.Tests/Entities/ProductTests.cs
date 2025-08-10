using Wrecept.Domain;

namespace Wrecept.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_Sets_Properties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var price = new Money("HUF", 100m);
        var tax = new TaxRate(0.27m);

        // Act
        var product = new Product(id, "Item", price, tax);

        // Assert
        Assert.Equal(id, product.Id);
        Assert.Equal("Item", product.Name);
        Assert.Equal(price, product.Price);
        Assert.Equal(tax, product.TaxRate);
    }

    [Fact]
    public void Constructor_EmptyName_Throws()
    {
        // Arrange
        var price = new Money("HUF", 100m);
        var tax = new TaxRate(0.27m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Product(Guid.NewGuid(), "", price, tax));
    }

    [Fact]
    public void Constructor_NullPrice_Throws()
    {
        // Arrange
        var tax = new TaxRate(0.27m);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Product(Guid.NewGuid(), "Item", null!, tax));
    }
}
