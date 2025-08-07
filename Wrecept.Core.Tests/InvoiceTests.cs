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
}
