using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Wrecept.Wpf.Converters;
using Xunit;

namespace Wrecept.Tests;

public class QuantityToStyleConverterTests
{
    [Fact]
    public void Convert_ReturnsNegativeBrush_ForNegative()
    {
        var red = new SolidColorBrush(Colors.Red);
        var c = new QuantityToStyleConverter { NegativeBrush = red };
        var result = c.Convert(-5m, typeof(Brush), null, CultureInfo.InvariantCulture);
        Assert.Same(red, result);
    }

    [Fact]
    public void Convert_ReturnsNormalBrush_ForNonNegativeOrInvalid()
    {
        var normal = new SolidColorBrush(Colors.White);
        var c = new QuantityToStyleConverter { NormalBrush = normal };
        Assert.Same(normal, c.Convert(0m, typeof(Brush), null, CultureInfo.InvariantCulture));
        Assert.Same(normal, c.Convert("x", typeof(Brush), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_ReturnsDoNothing()
    {
        var c = new QuantityToStyleConverter();
        Assert.Equal(Binding.DoNothing, c.ConvertBack(Brushes.Red, typeof(decimal), null, CultureInfo.InvariantCulture));
    }
}
