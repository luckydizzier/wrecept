using Wrecept.Domain;

namespace Wrecept.Domain.Tests;

public class InvoiceTests
{
    private static Product SampleProduct(string currency = "USD") => new(Guid.NewGuid(), "Prod", new Money(currency, 1m), new TaxRate(Guid.NewGuid(), "T", 0.2m));
    private static InvoiceLine SampleLine(string currency = "USD", int qty = 1, decimal price = 1m)
    {
        var product = SampleProduct(currency);
        return new InvoiceLine(product, qty, new Money(currency, price));
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        var customer = new Customer(Guid.NewGuid(), "Cust");
        var line = SampleLine();
        Assert.Throws<ArgumentException>(() => new Invoice(Guid.Empty, customer, new[] { line }));
    }

    [Fact]
    public void Constructor_NullCustomer_Throws()
    {
        var line = SampleLine();
        Assert.Throws<ArgumentNullException>(() => new Invoice(Guid.NewGuid(), null!, new[] { line }));
    }

    [Fact]
    public void Constructor_NullLines_Throws()
    {
        var customer = new Customer(Guid.NewGuid(), "Cust");
        Assert.Throws<ArgumentNullException>(() => new Invoice(Guid.NewGuid(), customer, null!));
    }

    [Fact]
    public void Constructor_EmptyLines_Throws()
    {
        var customer = new Customer(Guid.NewGuid(), "Cust");
        Assert.Throws<ArgumentException>(() => new Invoice(Guid.NewGuid(), customer, Array.Empty<InvoiceLine>()));
    }

    [Fact]
    public void Constructor_InconsistentCurrencies_Throws()
    {
        var customer = new Customer(Guid.NewGuid(), "Cust");
        var line1 = SampleLine("USD");
        var line2 = SampleLine("EUR");
        Assert.Throws<InvalidOperationException>(() => new Invoice(Guid.NewGuid(), customer, new[] { line1, line2 }));
    }

    [Fact]
    public void Total_EqualsSumOfLines()
    {
        var customer = new Customer(Guid.NewGuid(), "Cust");
        var line1 = SampleLine("USD", 2, 5m); // total 10
        var line2 = SampleLine("USD", 1, 7m); // total 7
        var invoice = new Invoice(Guid.NewGuid(), customer, new[] { line1, line2 });
        Assert.Equal(17m, invoice.Total.Amount);
        Assert.Equal("USD", invoice.Total.Currency);
    }
}
