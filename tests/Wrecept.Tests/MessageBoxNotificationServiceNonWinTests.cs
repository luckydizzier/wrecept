#if !WINDOWS
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class MessageBoxNotificationServiceNonWinTests
{
    [Fact]
    public void NonWindowsImplementation_NoThrow()
    {
        var svc = new MessageBoxNotificationService();
        svc.ShowError("err");
        svc.ShowInfo("info");
        Assert.True(svc.Confirm("ok"));
    }
}
#endif
