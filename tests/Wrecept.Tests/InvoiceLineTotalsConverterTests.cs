using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Wrecept.Core.Models;
using Wrecept.Wpf.Converters;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceLineTotalsConverterTests
{
    private readonly Guid _taxId = Guid.NewGuid();
    private readonly List<TaxRate> _taxes;

    public InvoiceLineTotalsConverterTests()
    {
        _taxes = new List<TaxRate>
        {
            new() { Id = _taxId, Percentage = 27m }
        };
    }

    [Fact]
    public void Convert_ReturnsExpectedTotals()
    {
        var conv = new InvoiceLineTotalsConverter { Mode = InvoiceLineTotalsConverterMode.Net };
        object[] values = { 2m, 10m, _taxId, false, _taxes };

        var net = conv.Convert(values, typeof(decimal), null, CultureInfo.InvariantCulture);
        Assert.Equal(20m, net);

        conv.Mode = InvoiceLineTotalsConverterMode.Vat;
        var vat = conv.Convert(values, typeof(decimal), null, CultureInfo.InvariantCulture);
        Assert.Equal(5.4m, vat);

        conv.Mode = InvoiceLineTotalsConverterMode.Gross;
        var gross = conv.Convert(values, typeof(decimal), null, CultureInfo.InvariantCulture);
        Assert.Equal(25.4m, gross);
    }

    [Fact]
    public void Convert_NegativeQuantity_ReturnsNegativeTotals()
    {
        var conv = new InvoiceLineTotalsConverter { Mode = InvoiceLineTotalsConverterMode.Net };
        object[] values = { -3m, 10m, _taxId, false, _taxes };
        var result = conv.Convert(values, typeof(decimal), null, CultureInfo.InvariantCulture);
        Assert.Equal(-30m, result);
    }

    [Fact]
    public void Convert_ReturnsDoNothing_OnInvalidInput()
    {
        var conv = new InvoiceLineTotalsConverter();
        var culture = CultureInfo.InvariantCulture;

        var resultShort = conv.Convert(new object[] { 1m, 2m }, typeof(decimal), null, culture);
        Assert.Equal(Binding.DoNothing, resultShort);

        var resultType = conv.Convert(new object[] { 1m, "2", _taxId, false, _taxes }, typeof(decimal), null, culture);
        Assert.Equal(Binding.DoNothing, resultType);
    }

    [Fact]
    public void ConvertBack_ReturnsEmptyArray()
    {
        var conv = new InvoiceLineTotalsConverter();
        var back = conv.ConvertBack(10m, Type.EmptyTypes, null, CultureInfo.InvariantCulture);
        Assert.Empty(back);
    }
}
