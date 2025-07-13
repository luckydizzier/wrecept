using System.Windows;
using System.Windows.Threading;
using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace Wrecept.Tests.ViewModels;

public class StatusBarViewModelTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void Constructor_StartsTimer()
    {
        EnsureApp();
        var vm = new StatusBarViewModel();
        Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
        Assert.False(string.IsNullOrWhiteSpace(vm.DateTime));
    }
}
