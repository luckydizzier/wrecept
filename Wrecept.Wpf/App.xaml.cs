using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core;
using Wrecept.Storage;

namespace Wrecept.Wpf;

public partial class App : Application
{
    public IServiceProvider Services { get; }

    public App()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        Services = serviceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dataDir);
        var dbPath = Path.Combine(dataDir, "app.db");

        services.AddCore();
        services.AddStorage(dbPath);
        services.AddSingleton<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var window = Services.GetRequiredService<MainWindow>();
        window.Show();
    }
}
