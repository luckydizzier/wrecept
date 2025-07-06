using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Wrecept.Wpf.Converters;
using Xunit;

namespace Wrecept.Tests;

public class StringNullOrEmptyToVisibilityConverterTests
{
    [Fact]
    public void Convert_ReturnsVisible_ForNullOrEmpty()
    {
        var c = new StringNullOrEmptyToVisibilityConverter();
        Assert.Equal(Visibility.Visible, c.Convert(null!, typeof(Visibility), null, CultureInfo.InvariantCulture));
        Assert.Equal(Visibility.Visible, c.Convert(string.Empty, typeof(Visibility), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Convert_ReturnsCollapsed_ForNonEmpty()
    {
        var c = new StringNullOrEmptyToVisibilityConverter();
        Assert.Equal(Visibility.Collapsed, c.Convert("x", typeof(Visibility), null, CultureInfo.InvariantCulture));
        Assert.Equal(Visibility.Collapsed, c.Convert(42, typeof(Visibility), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Convert_Inverts_WhenSet()
    {
        var c = new StringNullOrEmptyToVisibilityConverter { Invert = true };
        Assert.Equal(Visibility.Collapsed, c.Convert(null!, typeof(Visibility), null, CultureInfo.InvariantCulture));
        Assert.Equal(Visibility.Visible, c.Convert("x", typeof(Visibility), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_ReturnsDoNothing()
    {
        var c = new StringNullOrEmptyToVisibilityConverter();
        Assert.Equal(Binding.DoNothing, c.ConvertBack(Visibility.Visible, typeof(string), null, CultureInfo.InvariantCulture));
    }
}
