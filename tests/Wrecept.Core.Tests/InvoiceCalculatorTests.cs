using System;
using System.Collections.Generic;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class InvoiceCalculatorTests
{
    [Fact]
    public void Calculate_ComputesTotals()
    {
        var rate27 = new TaxRate { Id = Guid.NewGuid(), Percentage = 27 };
        var rate0 = new TaxRate { Id = Guid.NewGuid(), Percentage = 0 };
        var invoice = new Invoice
        {
            Items = new List<InvoiceItem>
            {
                new() { Quantity = 2, UnitPrice = 100m, TaxRate = rate27 },
                new() { Quantity = 1, UnitPrice = 50m, TaxRate = rate0 }
            }
        };
        var calc = new InvoiceCalculator();

        var result = calc.Calculate(invoice);

        Assert.Equal(250m, result.TotalNet);
        Assert.Equal(54m, result.TotalTax);
        Assert.Equal(304m, result.TotalGross);
        Assert.Equal(200m, result.PerTaxRateBreakdown[rate27.Id].Net);
        Assert.Equal(54m, result.PerTaxRateBreakdown[rate27.Id].Tax);
        Assert.Equal(254m, result.PerTaxRateBreakdown[rate27.Id].Gross);
        Assert.Equal(50m, result.PerTaxRateBreakdown[rate0.Id].Gross);
    }

    [Fact]
    public void Calculate_MissingTaxRate_Throws()
    {
        var invoice = new Invoice
        {
            Items = new List<InvoiceItem>
            {
                new() { Quantity = 1, UnitPrice = 10m }
            }
        };
        var calc = new InvoiceCalculator();

        Assert.Throws<InvalidOperationException>(() => calc.Calculate(invoice));
    }
}
