using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wrecept.Wpf.Converters;

public class StringNullOrEmptyToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool empty = value is null || string.IsNullOrEmpty(value.ToString());
        if (Invert) empty = !empty;
        return empty ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
