using InvoiceApp.Core.Utilities;
using Xunit;

namespace InvoiceApp.Core.Tests.Utilities;

public class SimpleMathTests
{
    [Theory]
    [InlineData(2, 3, 6)]
    [InlineData(-2, 3, -6)]
    [InlineData(0, 5, 0)]
    public void Multiply_ReturnsProduct(int a, int b, int expected)
    {
        var result = SimpleMath.Multiply(a, b);
        Assert.Equal(expected, result);
    }
}
