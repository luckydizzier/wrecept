using Wrecept.Core.Utilities;
using Xunit;

namespace Wrecept.Core.Tests.Utilities;

public class ProgressReportTests
{
    [Fact]
    public void DefaultValues_AreZeroAndEmpty()
    {
        var report = new ProgressReport();

        Assert.Equal(0, report.GlobalPercent);
        Assert.Equal(0, report.SubtaskPercent);
        Assert.Equal(string.Empty, report.Message);
    }
}
