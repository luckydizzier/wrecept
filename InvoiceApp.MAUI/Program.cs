using Microsoft.Maui;
using Microsoft.Maui.Hosting;


namespace InvoiceApp.MAUI;

public class Program : MauiWinUIApplication
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    static void Main(string[] args)
    {
        var app = new Program();
        app.Run(args);
    }
}
