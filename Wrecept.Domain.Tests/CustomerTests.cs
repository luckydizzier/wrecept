using Wrecept.Domain;

namespace Wrecept.Domain.Tests;

public class CustomerTests
{
    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Customer(Guid.Empty, "Name"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => new Customer(Guid.NewGuid(), name));
    }

    [Fact]
    public void Constructor_Valid_SetsProperties()
    {
        var id = Guid.NewGuid();
        var customer = new Customer(id, "Alice");
        Assert.Equal(id, customer.Id);
        Assert.Equal("Alice", customer.Name);
    }
}
