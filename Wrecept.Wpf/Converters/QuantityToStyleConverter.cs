using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Wrecept.Wpf.Converters;

public class QuantityToStyleConverter : IValueConverter
{
    public Brush NegativeBrush { get; set; } = Brushes.LightPink;
    public Brush NormalBrush { get; set; } = Brushes.Transparent;

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IConvertible conv)
        {
            try
            {
                if (conv.ToDecimal(culture) < 0)
                    return NegativeBrush;
            }
            catch (FormatException) { }
        }
        return NormalBrush;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
