using System.Windows;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class NavigationServiceTests
{
    private class AutoCloseWindow : Window
    {
        public AutoCloseWindow()
        {
            Loaded += (_, _) => { DialogResult = true; Close(); };
        }
    }

    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void ShowCenteredDialog_CentersAndReturnsTrue()
    {
        EnsureApp();
        Application.Current.MainWindow = new Window { Left = 10, Top = 20, Width = 200, Height = 200 };
        var dlg = new AutoCloseWindow { Width = 100, Height = 100 };

        var result = NavigationService.ShowCenteredDialog(dlg);

        Assert.True(result);
        Assert.Equal(Application.Current.MainWindow, dlg.Owner);
        Assert.Equal(WindowStartupLocation.Manual, dlg.WindowStartupLocation);
        var expectedLeft = Application.Current.MainWindow.Left +
            (Application.Current.MainWindow.Width - dlg.Width) / 2;
        var expectedTop = Application.Current.MainWindow.Top +
            (Application.Current.MainWindow.Height - dlg.Height) / 2;
        Assert.Equal(expectedLeft, dlg.Left);
        Assert.Equal(expectedTop, dlg.Top);
    }
}
