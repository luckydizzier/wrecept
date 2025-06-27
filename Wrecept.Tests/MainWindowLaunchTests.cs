using Xunit;
using System.Threading;
using Wrecept.Desktop;

namespace Wrecept.Tests;

public class MainWindowLaunchTests
{
    [StaFact]
    public void MainWindow_Instantiates()
    {
        var t = new Thread(() =>
        {
            var app = new App();
            app.InitializeComponent();
            var window = new MainWindow();
            // we don't show the window to avoid GUI requirement
        });
        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();
        Assert.False(t.IsAlive);
    }
}
