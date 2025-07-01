using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wrecept.Wpf.Converters;

public class BooleanToRowDetailsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b
            ? DataGridRowDetailsVisibilityMode.Visible
            : DataGridRowDetailsVisibilityMode.Collapsed;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is DataGridRowDetailsVisibilityMode mode && mode != DataGridRowDetailsVisibilityMode.Collapsed;
}
