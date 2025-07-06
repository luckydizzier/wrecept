using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Wrecept.Wpf.Converters;
using Xunit;

namespace Wrecept.Tests;

public class NegativeValueForegroundConverterTests
{
    [Fact]
    public void Convert_ReturnsNegativeBrush_ForNegative()
    {
        var blue = new SolidColorBrush(Colors.Blue);
        var c = new NegativeValueForegroundConverter { NegativeBrush = blue };
        var result = c.Convert(-1m, typeof(Brush), null, CultureInfo.InvariantCulture);
        Assert.Same(blue, result);
    }

    [Fact]
    public void Convert_ReturnsPositiveBrush_ForNonNegativeOrInvalid()
    {
        var green = new SolidColorBrush(Colors.Green);
        var c = new NegativeValueForegroundConverter { PositiveBrush = green };
        Assert.Same(green, c.Convert(0m, typeof(Brush), null, CultureInfo.InvariantCulture));
        Assert.Same(green, c.Convert("x", typeof(Brush), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_ReturnsDoNothing()
    {
        var c = new NegativeValueForegroundConverter();
        Assert.Equal(Binding.DoNothing, c.ConvertBack(Brushes.Red, typeof(decimal), null, CultureInfo.InvariantCulture));
    }
}
