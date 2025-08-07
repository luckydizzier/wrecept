using Wrecept.Domain;

namespace Wrecept.Domain.Tests;

public class TaxRateTests
{
    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(() => new TaxRate(Guid.Empty, "Standard", 0.2m));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => new TaxRate(Guid.NewGuid(), name, 0.2m));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Constructor_RateOutOfRange_Throws(double rate)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TaxRate(Guid.NewGuid(), "Standard", (decimal)rate));
    }

    [Fact]
    public void Constructor_Valid_SetsProperties()
    {
        var id = Guid.NewGuid();
        var tax = new TaxRate(id, "Standard", 0.27m);
        Assert.Equal(id, tax.Id);
        Assert.Equal("Standard", tax.Name);
        Assert.Equal(0.27m, tax.Rate);
    }
}
