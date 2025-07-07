using Wrecept.Core.Utilities;
using Xunit;

namespace Wrecept.Core.Tests;

public class SimpleMathTests
{
    [Fact]
    public void Multiply_ReturnsProduct()
    {
        Assert.Equal(12, SimpleMath.Multiply(3, 4));
    }
}
