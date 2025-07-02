using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Wrecept.Core.Models;

namespace Wrecept.Wpf.Converters;

public class InvoiceLineTotalsConverter : IMultiValueConverter
{
    public InvoiceLineTotalsConverterMode Mode { get; set; } = InvoiceLineTotalsConverterMode.Net;

    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length < 5)
            return Binding.DoNothing;
        if (values[0] is not decimal qty ||
            values[1] is not decimal price ||
            values[2] is not Guid taxId ||
            values[3] is not bool isGross ||
            values[4] is not IEnumerable<TaxRate> taxes)
            return Binding.DoNothing;

        var rate = taxes.FirstOrDefault(t => t.Id == taxId)?.Percentage ?? 0m;
        var netUnit = isGross ? price / (1 + rate / 100m) : price;
        var net = qty * netUnit;
        var vat = net * rate / 100m;
        var gross = net + vat;
        return Mode switch
        {
            InvoiceLineTotalsConverterMode.Net => net,
            InvoiceLineTotalsConverterMode.Vat => vat,
            InvoiceLineTotalsConverterMode.Gross => gross,
            _ => net,
        };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        // A visszairányú konverzióra jelenleg nincs szükség, ezért "DoNothing" jelzés
        // mellett üres tömbbel térünk vissza, hogy a binding ne okozzon hibát.
        return Array.Empty<object>();
    }
}

public enum InvoiceLineTotalsConverterMode
{
    Net,
    Vat,
    Gross
}
