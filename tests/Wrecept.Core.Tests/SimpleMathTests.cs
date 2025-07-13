using InvoiceApp.Core.Utilities;
using Xunit;

namespace InvoiceApp.Core.Tests;

public class SimpleMathTests
{
    [Fact]
    public void Multiply_ReturnsProduct()
    {
        Assert.Equal(12, SimpleMath.Multiply(3, 4));
    }
}
