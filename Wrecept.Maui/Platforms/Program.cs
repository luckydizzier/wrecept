using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Wrecept.Maui;

public class Program : MauiApplication
{
    public static void Main(string[] args)
    {
        var app = MauiProgram.CreateMauiApp();
        app.Run(args);
    }

    public Program() : base() { }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
