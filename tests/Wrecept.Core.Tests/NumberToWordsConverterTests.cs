using Wrecept.Core.Utilities;
using Xunit;

namespace Wrecept.Core.Tests;

public class NumberToWordsConverterTests
{
    [Theory]
    [InlineData(0, "nulla")]
    [InlineData(-5, "mínusz öt")]
    [InlineData(1000000, "egy millió")]
    public void Convert_ReturnsExpected(long value, string expected)
    {
        Assert.Equal(expected, NumberToWordsConverter.Convert(value));
    }
}
