using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.Tests.ViewModels;

public class ProgressViewModelTests
{
    [Fact]
    public void DefaultValues_AreZero()
    {
        var vm = new ProgressViewModel();

        Assert.Equal(0, vm.GlobalProgress);
        Assert.Equal(0, vm.SubProgress);
        Assert.Equal(string.Empty, vm.StatusMessage);
        Assert.Null(vm.CancelCommand);
    }
}
