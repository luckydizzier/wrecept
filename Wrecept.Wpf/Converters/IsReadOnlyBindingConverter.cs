using System;
using System.Globalization;
using System.Windows.Data;

namespace Wrecept.Wpf.Converters;

public class IsReadOnlyBindingConverter : IValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && (Invert ? !b : b);

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
