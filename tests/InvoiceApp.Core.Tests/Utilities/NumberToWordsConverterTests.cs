using InvoiceApp.Core.Utilities;
using Xunit;

namespace InvoiceApp.Core.Tests.Utilities;

public class NumberToWordsConverterTests
{
    [Fact]
    public void Convert_Zero_ReturnsNulla()
    {
        var result = NumberToWordsConverter.Convert(0);
        Assert.Equal("nulla", result);
    }

    [Fact]
    public void Convert_Negative_ReturnsMinusPrefix()
    {
        var result = NumberToWordsConverter.Convert(-1);
        Assert.Equal("mínusz egy", result);
    }

    [Fact]
    public void Convert_CompoundNumber_ReturnsExpectedString()
    {
        var result = NumberToWordsConverter.Convert(123);
        Assert.Equal("száz húszhárom", result);
    }

    [Fact]
    public void Convert_LargeNumber_ReturnsExpectedString()
    {
        var result = NumberToWordsConverter.Convert(1100000);
        Assert.Equal("egy millió száz ezer", result);
    }
}
