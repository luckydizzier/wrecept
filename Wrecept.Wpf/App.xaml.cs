using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core;
using Wrecept.Storage;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Views.Controls;
namespace Wrecept.Wpf;

public partial class App : Application
{
    public IServiceProvider Services { get; }
    public static IServiceProvider Provider => ((App)Current).Services;
    public static string DbPath { get; private set; } = string.Empty;

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
        DbPath = dbPath;

        services.AddCore();
        services.AddStorage(dbPath);

        services.AddTransient<StageViewModel>();
        services.AddTransient<InvoiceEditorViewModel>();
        services.AddTransient<ProductMasterViewModel>();
        services.AddTransient<ProductGroupMasterViewModel>();
        services.AddTransient<SupplierMasterViewModel>();
        services.AddTransient<TaxRateMasterViewModel>();
        services.AddTransient<PaymentMethodMasterViewModel>();
        services.AddTransient<UnitMasterViewModel>();
        services.AddTransient<AboutViewModel>();
        services.AddTransient<PlaceholderViewModel>();
        services.AddSingleton<StatusBarViewModel>();
        services.AddTransient<StageView>();
        services.AddTransient<InvoiceEditorView>();
        services.AddTransient<ProductMasterView>();
        services.AddTransient<ProductGroupMasterView>();
        services.AddTransient<SupplierMasterView>();
        services.AddTransient<TaxRateMasterView>();
        services.AddTransient<PaymentMethodMasterView>();
        services.AddTransient<UnitMasterView>();
        services.AddTransient<AboutView>();
        services.AddTransient<PlaceholderView>();
        services.AddTransient<Views.Controls.StatusBar>();

        services.AddSingleton<MainWindow>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var logger = Services.GetRequiredService<Wrecept.Core.Services.ILogService>();
        var status = await Wrecept.Storage.Data.DataSeeder.SeedAsync(DbPath, logger);
        if (status == Wrecept.Storage.Data.SeedStatus.Failed)
        {
            MessageBox.Show(
                "Az adatbázis nem inicializálható. Részletek a logs/startup.log fájlban.",
                "Indítási hiba",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        else if (status != Wrecept.Storage.Data.SeedStatus.None)
        {
            MessageBox.Show(
                "A(z) app.db hiányzott vagy csak mintaadatokat tartalmazott. Mintaadatok betöltve.",
                "Első indítás",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        var window = Services.GetRequiredService<MainWindow>();
        window.Show();
    }
}
