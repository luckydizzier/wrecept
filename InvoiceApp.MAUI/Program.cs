using Microsoft.Maui;
using Microsoft.Maui.Hosting;
namespace InvoiceApp.MAUI;
public class Program : MauiApplication
{
    public Program() : base()
    {
    }
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    static void Main(string[] args) => new Program().Run(args);
}

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        return builder.Build();
    }
}
