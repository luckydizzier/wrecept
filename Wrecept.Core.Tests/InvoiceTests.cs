using Wrecept.Core.Models;

namespace Wrecept.Core.Tests;

public class InvoiceTests
{
    [Fact]
    public void Invoice_Initializes_WithEmptyItems()
    {
        var invoice = new Invoice();
        Assert.NotNull(invoice.Items);
    }

    [Fact]
    public void Invoice_HasActiveStatusByDefault()
    {
        var invoice = new Invoice();
        Assert.Equal(InvoiceStatus.Active, invoice.Status);
        Assert.Equal("A", invoice.StatusMarker);
        Assert.Equal("Active invoice", invoice.StatusDescription);
    }
}
