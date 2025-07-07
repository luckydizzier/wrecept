using System.Windows;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Xunit;

namespace Wrecept.Tests;

public class SetupAndSeedWindowTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void SetupWindow_Ok_SetsDialogResultTrue()
    {
        EnsureApp();
        var vm = new SetupViewModel("db", "cfg");
        var win = new SetupWindow { DataContext = vm };

        vm.OkCommand.Execute(win);

        Assert.True(win.DialogResult);
    }

    [StaFact]
    public void SetupWindow_Cancel_SetsDialogResultFalse()
    {
        EnsureApp();
        var vm = new SetupViewModel("db", "cfg");
        var win = new SetupWindow { DataContext = vm };

        vm.CancelCommand.Execute(win);

        Assert.False(win.DialogResult);
    }

    [StaFact]
    public void SeedOptionsWindow_Ok_SetsDialogResultTrue()
    {
        EnsureApp();
        var vm = new SeedOptionsViewModel();
        var win = new SeedOptionsWindow { DataContext = vm };

        vm.OkCommand.Execute(win);

        Assert.True(win.DialogResult);
    }

    [StaFact]
    public void SeedOptionsWindow_Cancel_SetsDialogResultFalse()
    {
        EnsureApp();
        var vm = new SeedOptionsViewModel();
        var win = new SeedOptionsWindow { DataContext = vm };

        vm.CancelCommand.Execute(win);

        Assert.False(win.DialogResult);
    }
}
