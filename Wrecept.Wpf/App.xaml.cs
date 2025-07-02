using System;
using System.IO;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core;
using Wrecept.Core.Services;
using Wrecept.Core.Entities;
using Wrecept.Storage;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Views.Controls;
using Wrecept.Wpf.Services;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Utilities;
using Wrecept.Storage.Data;
namespace Wrecept.Wpf;

public partial class App : Application
{
public static IServiceProvider? Services { get; private set; }
public static IServiceProvider Provider => Services ?? throw new InvalidOperationException("App services not initialized");
    public static string DbPath { get; private set; } = string.Empty;
    public static string UserInfoPath { get; private set; } = string.Empty;
    public static string SettingsPath { get; private set; } = string.Empty;

    public App()
    {
        EnsureServicesInitialized();
    }

    private static void EnsureServicesInitialized()
    {
        if (Services != null)
            return;

        var settings = LoadSettings();
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, settings);
        Services = serviceCollection.BuildServiceProvider();
        ThemeManager.ApplyDarkTheme(false);
    }

    private static AppSettings LoadSettings()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dataDir);
        SettingsPath = Path.Combine(dataDir, "settings.json");
        if (File.Exists(SettingsPath))
        {
            using var stream = File.OpenRead(SettingsPath);
            return JsonSerializer.Deserialize<AppSettings>(stream) ?? new AppSettings();
        }

        var defaultDb = Path.Combine(Environment.CurrentDirectory, "app.db");
        var defaultCfg = Path.Combine(Environment.CurrentDirectory, "wrecept.json");
        var vm = new SetupViewModel(defaultDb, defaultCfg);
        var win = new SetupWindow { DataContext = vm };
        win.ShowDialog();

        var settings = new AppSettings
        {
            DatabasePath = vm.DatabasePath,
            UserInfoPath = vm.ConfigPath
        };
        using var save = File.Create(SettingsPath);
        JsonSerializer.Serialize(save, settings, new JsonSerializerOptions { WriteIndented = true });
        return settings;
    }

    private static void ConfigureServices(IServiceCollection services, AppSettings settings)
    {
        DbPath = settings.DatabasePath;
        UserInfoPath = settings.UserInfoPath;

        services.AddCore();
        services.AddStorage(DbPath, UserInfoPath, SettingsPath);

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
        services.AddTransient<ScreenModeViewModel>();
        services.AddTransient<AboutViewModel>();
        services.AddTransient<PlaceholderViewModel>();
        services.AddSingleton<StatusBarViewModel>();
        services.AddSingleton<INotificationService, MessageBoxNotificationService>();
        services.AddSingleton<ScreenModeManager>();
        services.AddTransient<ProgressViewModel>();
        services.AddTransient<StartupWindow>();
        services.AddTransient<ScreenModeWindow>();
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

        EnsureServicesInitialized();

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var orchestrator = Provider.GetRequiredService<StartupOrchestrator>();
        using var cts = new CancellationTokenSource();
        var progressVm = Provider.GetRequiredService<ProgressViewModel>();
        progressVm.CancelCommand = new RelayCommand(() => cts.Cancel());
        var progress = new Progress<ProgressReport>(r =>
        {
            progressVm.GlobalProgress = r.GlobalPercent;
            progressVm.SubProgress = r.SubtaskPercent;
            progressVm.StatusMessage = r.Message;
        });

        if (await orchestrator.DatabaseEmptyAsync(cts.Token))
        {
            var result = MessageBox.Show(
                "Kérsz 100 példaszámlát 20 szállítótól, 500 termékkel, számlánként 5-60 tétellel?",
                "Mintadatok",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                var startupWindow = Provider.GetRequiredService<StartupWindow>();
                startupWindow.DataContext = progressVm;
                startupWindow.Show();
                var status = await orchestrator.SeedAsync(progress, cts.Token);
                startupWindow.Close();

                if (status == SeedStatus.Failed)
                {
                    MessageBox.Show(
                        "Az adatbázis nem inicializálható. Részletek a logs/startup.log fájlban.",
                        "Indítási hiba",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        var window = Provider.GetRequiredService<MainWindow>();
        MainWindow = window;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        window.Show();
    }
}
