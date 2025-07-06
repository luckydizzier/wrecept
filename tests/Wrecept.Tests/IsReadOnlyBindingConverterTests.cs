using System.Globalization;
using System.Windows.Data;
using Wrecept.Wpf.Converters;
using Xunit;

namespace Wrecept.Tests;

public class IsReadOnlyBindingConverterTests
{
    [Fact]
    public void Convert_ReturnsValue()
    {
        var c = new IsReadOnlyBindingConverter();
        Assert.True((bool)c.Convert(true, typeof(bool), null, CultureInfo.InvariantCulture));
        Assert.False((bool)c.Convert(false, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Convert_Inverts_WhenSet()
    {
        var c = new IsReadOnlyBindingConverter { Invert = true };
        Assert.False((bool)c.Convert(true, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Convert_ReturnsFalse_ForNonBool()
    {
        var c = new IsReadOnlyBindingConverter();
        Assert.False((bool)c.Convert("x", typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_ReturnsDoNothing()
    {
        var c = new IsReadOnlyBindingConverter();
        Assert.Equal(Binding.DoNothing, c.ConvertBack(true, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
