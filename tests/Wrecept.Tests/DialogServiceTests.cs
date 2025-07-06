using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class DialogServiceTests
{
    private class AutoCloseView : FrameworkElement
    {
        public AutoCloseView()
        {
            Loaded += (_, _) =>
            {
                if (Window.GetWindow(this) is Window w)
                {
                    w.DialogResult = true;
                    w.Close();
                }
            };
        }
    }

    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void EditEntity_ShowsDialogAndReturnsTrue()
    {
        EnsureApp();
        Application.Current.MainWindow = new Window();
        var vm = new object();
        var result = DialogService.EditEntity<AutoCloseView, object>(vm, new RelayCommand(() => { }), new RelayCommand(() => { }));
        Assert.True(result);
    }
}
