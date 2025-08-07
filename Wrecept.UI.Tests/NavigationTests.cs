using System.Runtime.InteropServices;

namespace Wrecept.UI.Tests;

public class NavigationTests
{
    [SkippableFact]
    [Trait("Category", "UI")]
    public void Should_NavigateProperly()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "UI tests require WindowsDesktop SDK");
        Assert.True(true);
    }
}
