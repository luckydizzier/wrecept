using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class MessageBoxNotificationServiceTests
{
    [Fact]
    public void Confirm_ReturnsTrue()
    {
        var svc = new MessageBoxNotificationService();
        Assert.True(svc.Confirm("?"));
    }

    [Fact]
    public void ShowMethods_DoNotThrow()
    {
        var svc = new MessageBoxNotificationService();
        svc.ShowError("err");
        svc.ShowInfo("info");
    }
}
