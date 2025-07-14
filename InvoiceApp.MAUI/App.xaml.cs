using Microsoft.Maui.Controls;

namespace InvoiceApp.MAUI;

public partial class App : Application
{
    public App(MainPage mainPage, StartupOrchestrator startup)
    {
        InitializeComponent();
        MainPage = mainPage;
        _ = startup; // startup orchestration will run later
    }
}
