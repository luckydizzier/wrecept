using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.Tests.ViewModels;

public class StatusBarViewModelTests
{
    [Fact]
    public async Task Constructor_StartsTimer()
    {
        var vm = new StatusBarViewModel();
        await MainThread.InvokeOnMainThreadAsync(async () => await Task.Delay(50));
        Assert.False(string.IsNullOrWhiteSpace(vm.DateTime));
    }
}
