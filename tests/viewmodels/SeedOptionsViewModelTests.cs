using System.Windows;
using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.Tests.ViewModels;

public class SeedOptionsViewModelTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void OkCommand_SetsDialogResult()
    {
        EnsureApp();
        var vm = new SeedOptionsViewModel();
        var window = new Window();
        vm.OkCommand.Execute(window);
        Assert.False(window.IsVisible);
        Assert.True(window.DialogResult);
    }

    [StaFact]
    public void CancelCommand_SetsDialogResultFalse()
    {
        EnsureApp();
        var vm = new SeedOptionsViewModel();
        var window = new Window();
        vm.CancelCommand.Execute(window);
        Assert.False(window.IsVisible);
        Assert.False(window.DialogResult);
    }
}
