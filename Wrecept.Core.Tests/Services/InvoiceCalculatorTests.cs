using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class InvoiceCalculatorTests
{
    private static Invoice CreateInvoice(bool isGross, decimal quantity, decimal unitPrice, decimal taxPercent)
    {
        var taxRateId = Guid.NewGuid();
        var taxRate = new TaxRate { Id = taxRateId, Percentage = taxPercent };
        var product = new Product { Id = 1, TaxRate = taxRate };
        var item = new InvoiceItem { Product = product, Quantity = quantity, UnitPrice = unitPrice };
        return new Invoice
        {
            IsGross = isGross,
            Items = new List<InvoiceItem> { item }
        };
    }

    [Fact]
    public void Calculate_NetInvoice_ReturnsCorrectTotals()
    {
        var invoice = CreateInvoice(false, 2, 100, 27);
        var calc = new InvoiceCalculator();

        var result = calc.Calculate(invoice);

        Assert.Equal(200, result.TotalNet);
        Assert.Equal(54, result.TotalTax);
        Assert.Equal(254, result.TotalGross);
    }

    [Fact]
    public void Calculate_GrossInvoice_ReturnsCorrectTotals()
    {
        var invoice = CreateInvoice(true, 2, 127, 27);
        var calc = new InvoiceCalculator();

        var result = calc.Calculate(invoice);

        Assert.Equal(200, result.TotalNet);
        Assert.Equal(54, result.TotalTax);
        Assert.Equal(254, result.TotalGross);
    }

    [Fact]
    public void Calculate_NegativeQuantity_ReturnsNegativeTotals()
    {
        var invoice = CreateInvoice(false, -2, 100, 27);
        var calc = new InvoiceCalculator();

        var result = calc.Calculate(invoice);

        Assert.Equal(-200, result.TotalNet);
        Assert.Equal(-54, result.TotalTax);
        Assert.Equal(-254, result.TotalGross);
    }
}
