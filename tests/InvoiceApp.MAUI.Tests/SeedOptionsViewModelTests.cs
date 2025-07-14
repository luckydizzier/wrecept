using Microsoft.Maui.Controls;
using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.Tests.ViewModels;

public class SeedOptionsViewModelTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            _ = new Application();
    }

    [StaFact]
    public void OkCommand_SetsDialogResult()
    {
        EnsureApp();
        var vm = new SeedOptionsViewModel();
        bool? result = null;
        vm.DialogResult += r => result = r;
        vm.OkCommand.Execute(null);
        Assert.True(result);
    }

    [StaFact]
    public void CancelCommand_SetsDialogResultFalse()
    {
        EnsureApp();
        var vm = new SeedOptionsViewModel();
        bool? result = null;
        vm.DialogResult += r => result = r;
        vm.CancelCommand.Execute(null);
        Assert.False(result);
    }
}
