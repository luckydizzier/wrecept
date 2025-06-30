using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Wrecept.Wpf.Converters;

public class NegativeValueForegroundConverter : IValueConverter
{
    public Brush NegativeBrush { get; set; } = Brushes.Red;
    public Brush PositiveBrush { get; set; } = Brushes.Black;

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
        return PositiveBrush;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
