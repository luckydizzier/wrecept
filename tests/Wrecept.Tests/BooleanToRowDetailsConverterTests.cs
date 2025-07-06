using System.Globalization;
using System.Windows.Controls;
using Wrecept.Wpf.Converters;
using Xunit;

namespace Wrecept.Tests;

public class BooleanToRowDetailsConverterTests
{
    [Fact]
    public void Convert_ReturnsVisible_ForTrue()
    {
        var c = new BooleanToRowDetailsConverter();
        var result = c.Convert(true, typeof(DataGridRowDetailsVisibilityMode), null, CultureInfo.InvariantCulture);
        Assert.Equal(DataGridRowDetailsVisibilityMode.Visible, result);
    }

    [Fact]
    public void Convert_ReturnsCollapsed_ForFalseOrInvalid()
    {
        var c = new BooleanToRowDetailsConverter();
        Assert.Equal(DataGridRowDetailsVisibilityMode.Collapsed, c.Convert(false, typeof(object), null, CultureInfo.InvariantCulture));
        Assert.Equal(DataGridRowDetailsVisibilityMode.Collapsed, c.Convert(null!, typeof(object), null, CultureInfo.InvariantCulture));
        Assert.Equal(DataGridRowDetailsVisibilityMode.Collapsed, c.Convert("x", typeof(object), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_ReturnsTrue_WhenVisible()
    {
        var c = new BooleanToRowDetailsConverter();
        Assert.True((bool)c.ConvertBack(DataGridRowDetailsVisibilityMode.Visible, typeof(bool), null, CultureInfo.InvariantCulture));
        Assert.True((bool)c.ConvertBack(DataGridRowDetailsVisibilityMode.VisibleWhenSelected, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_ReturnsFalse_ForCollapsedOrInvalid()
    {
        var c = new BooleanToRowDetailsConverter();
        Assert.False((bool)c.ConvertBack(DataGridRowDetailsVisibilityMode.Collapsed, typeof(bool), null, CultureInfo.InvariantCulture));
        Assert.False((bool)c.ConvertBack(5, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
