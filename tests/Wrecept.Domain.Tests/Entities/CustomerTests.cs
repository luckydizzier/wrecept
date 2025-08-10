using Wrecept.Domain;

namespace Wrecept.Domain.Tests.Entities;

public class CustomerTests
{
    [Fact]
    public void Constructor_Sets_Properties()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var customer = new Customer(id, "Alice");

        // Assert
        Assert.Equal(id, customer.Id);
        Assert.Equal("Alice", customer.Name);
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(Guid.Empty, "Alice"));
    }

    [Fact]
    public void Constructor_EmptyName_Throws()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(Guid.NewGuid(), ""));
    }
}
