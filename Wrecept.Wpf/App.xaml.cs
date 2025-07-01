using System;
using System.IO;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core;
using Wrecept.Storage;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Views.Controls;
using Wrecept.Wpf.Services;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Utilities;
using Wrecept.Storage.Data;
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
        services.AddTransient<InvoiceLookupViewModel>();
        services.AddTransient<ProductMasterViewModel>();
        services.AddTransient<ProductGroupMasterViewModel>();
        services.AddTransient<SupplierMasterViewModel>();
        services.AddTransient<TaxRateMasterViewModel>();
        services.AddTransient<PaymentMethodMasterViewModel>();
        services.AddTransient<UnitMasterViewModel>();
        services.AddTransient<UserInfoViewModel>();
        services.AddTransient<AboutViewModel>();
        services.AddTransient<PlaceholderViewModel>();
        services.AddSingleton<StatusBarViewModel>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddTransient<ProgressViewModel>();
        services.AddTransient<StartupWindow>();
        services.AddTransient<StartupOrchestrator>();
        services.AddTransient<StageView>();
        services.AddTransient<InvoiceEditorView>();
        services.AddTransient<InvoiceLookupView>();
        services.AddTransient<ProductMasterView>();
        services.AddTransient<ProductGroupMasterView>();
        services.AddTransient<SupplierMasterView>();
        services.AddTransient<TaxRateMasterView>();
        services.AddTransient<PaymentMethodMasterView>();
        services.AddTransient<UnitMasterView>();
        services.AddTransient<UserInfoView>();
        services.AddTransient<AboutView>();
        services.AddTransient<PlaceholderView>();
        services.AddTransient<Views.Controls.StatusBar>();

        services.AddSingleton<MainWindow>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var orchestrator = Services.GetRequiredService<StartupOrchestrator>();
        var progressVm = Services.GetRequiredService<ProgressViewModel>();
        using var cts = new CancellationTokenSource();
        progressVm.CancelCommand = new RelayCommand(() => cts.Cancel());
        var progress = new Progress<ProgressReport>(r =>
        {
            progressVm.GlobalProgress = r.GlobalPercent;
            progressVm.SubProgress = r.SubtaskPercent;
            progressVm.StatusMessage = r.Message;
        });

        var startupWindow = Services.GetRequiredService<StartupWindow>();
        startupWindow.DataContext = progressVm;
        startupWindow.Show();
        var status = await orchestrator.RunAsync(progress, cts.Token);
        startupWindow.Close();

        if (status == SeedStatus.Failed)
        {
            MessageBox.Show(
                "Az adatbázis nem inicializálható. Részletek a logs/startup.log fájlban.",
                "Indítási hiba",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        else if (status != SeedStatus.None)
        {
            MessageBox.Show(
                "A(z) app.db hiányzott vagy csak mintaadatokat tartalmazott. Mintaadatok betöltve.",
                "Első indítás",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        var window = Services.GetRequiredService<MainWindow>();
        MainWindow = window;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        window.Show();
    }
}
