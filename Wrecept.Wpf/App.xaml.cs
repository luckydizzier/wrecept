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
using Wrecept.Storage.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Views.Controls;
using Wrecept.Wpf.Services;
using Wrecept.Core.Enums;
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
    public static string StatePath { get; private set; } = string.Empty;
    public static IEnvironmentService EnvironmentService { get; set; } = new EnvironmentService();
    public static Func<string, bool>? ConfirmOverride { get; set; }

    public App()
    {
    }

    private static async Task EnsureServicesInitializedAsync()
    {
        if (Services != null)
            return;

        var settings = await LoadSettingsAsync();
        var serviceCollection = new ServiceCollection();
        await ConfigureServicesAsync(serviceCollection, settings);
        Services = serviceCollection.BuildServiceProvider();
        NavigationService.State = Services.GetRequiredService<AppStateService>();
        ThemeManager.ApplyDarkTheme(false);
    }

    private static async Task<AppSettings> LoadSettingsAsync(
        INotificationService? notifications = null,
        ISetupFlow? setupFlow = null)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dataDir);
        SettingsPath = Path.Combine(dataDir, "settings.json");
        StatePath = Path.Combine(dataDir, "state.json");
        if (File.Exists(SettingsPath))
        {
            try
            {
                using var stream = File.OpenRead(SettingsPath);
                return JsonSerializer.Deserialize<AppSettings>(stream) ?? new AppSettings();
            }
            catch (Exception ex) when (ex is IOException || ex is JsonException)
            {
                var logger = new Wrecept.Storage.Services.LogService();
                await logger.LogError("LoadSettings", ex);
            }
        }

        var defaultDb = Path.Combine(Environment.CurrentDirectory, "app.db");
        var defaultCfg = Path.Combine(Environment.CurrentDirectory, "wrecept.json");

        notifications ??= new MessageBoxNotificationService();
        setupFlow ??= new SetupFlow();

        var confirm = ConfirmOverride ?? notifications.Confirm;
        if (!confirm("Biztos, hogy elölrõl kezded?"))
        {
            Current.Shutdown();
            EnvironmentService.Exit(0);
            return new AppSettings(); // alkalmazás kilép, de fordítás miatt értéket adunk
        }

        var setup = await setupFlow.RunAsync(defaultDb, defaultCfg, EnvironmentService);

        var userInfoService = new Wrecept.Storage.Services.UserInfoService(setup.ConfigPath);
        await userInfoService.SaveAsync(setup.Info);

        var settings = new AppSettings
        {
            DatabasePath = setup.DatabasePath,
            UserInfoPath = setup.ConfigPath
        };
        try
        {
            using var save = File.Create(SettingsPath);
            JsonSerializer.Serialize(save, settings, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex) when (ex is IOException || ex is JsonException)
        {
            var logger = new Wrecept.Storage.Services.LogService();
            await logger.LogError("SaveSettings", ex);
        }
        return settings;
    }

    private static async Task ConfigureServicesAsync(IServiceCollection services, AppSettings settings)
    {
        DbPath = settings.DatabasePath;
        UserInfoPath = settings.UserInfoPath;

        services.AddCore();
        await services.AddStorageAsync(DbPath, UserInfoPath, SettingsPath);


        services.AddSingleton<StageViewModel>();
        services.AddSingleton<InvoiceEditorViewModel>();
        services.AddSingleton<InvoiceLookupViewModel>();
        services.AddTransient<ProductMasterViewModel>();
        services.AddTransient<ProductGroupMasterViewModel>();
        services.AddTransient<SupplierMasterViewModel>();
        services.AddTransient<TaxRateMasterViewModel>();
        services.AddTransient<PaymentMethodMasterViewModel>();
        services.AddTransient<UnitMasterViewModel>();
        services.AddTransient<UserInfoViewModel>();
        services.AddTransient<UserInfoEditorViewModel>();
        services.AddTransient<ScreenModeViewModel>();
        services.AddTransient<AboutViewModel>();
        services.AddTransient<PlaceholderViewModel>();
        services.AddSingleton<StatusBarViewModel>();
        services.AddSingleton<AppStateService>(_ => new AppStateService(StatePath));
        services.AddSingleton<INotificationService, MessageBoxNotificationService>();
        services.AddTransient<IInvoiceExportService, PdfInvoiceExporter>();
        services.AddSingleton<ScreenModeManager>();
        services.AddSingleton<FocusManager>();
        services.AddSingleton<KeyboardManager>();
        services.AddTransient<StageMenuHandler>();
        services.AddTransient<StageMenuKeyboardHandler>();
        services.AddTransient<InvoiceEditorKeyboardHandler>();
        services.AddTransient<MasterDataKeyboardHandler>();
        services.AddTransient<ProgressViewModel>();
        services.AddTransient<SeedOptionsViewModel>();
        services.AddTransient<SeedOptionsWindow>();
        services.AddTransient<StartupWindow>();
        services.AddTransient<ScreenModeWindow>();
        services.AddTransient<StartupOrchestrator>();
        services.AddTransient<StageView>();
        services.AddTransient<InvoiceLookupView>();
        services.AddTransient<ProductMasterView>();
        services.AddTransient<ProductGroupMasterView>();
        services.AddTransient<SupplierMasterView>();
        services.AddTransient<TaxRateMasterView>();
        services.AddTransient<PaymentMethodMasterView>();
        services.AddTransient<UnitMasterView>();
        services.AddTransient<UserInfoView>();
        services.AddTransient<UserInfoWindow>();
        services.AddTransient<AboutView>();
        services.AddTransient<PlaceholderView>();
        services.AddTransient<Views.Controls.StatusBar>();

        services.AddSingleton<MainWindow>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        CancellationTokenSource? cts = null;
        ProgressViewModel? progressVm = null;

        try
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            await EnsureServicesInitializedAsync();
            await Provider.GetRequiredService<IDatabaseInitializer>().InitializeAsync();
            await Provider.GetRequiredService<AppStateService>().LoadAsync();

            var km = Provider.GetRequiredService<KeyboardManager>();
            var lookupVm = Provider.GetRequiredService<InvoiceLookupViewModel>();
            var lookupHandler = new InvoiceLookupKeyboardHandler(lookupVm);
            km.Register(AppInteractionState.BrowsingInvoices, lookupHandler);

            var stageHandler = Provider.GetRequiredService<StageMenuKeyboardHandler>();
            km.Register(AppInteractionState.MainMenu, stageHandler);

            var editorHandler = Provider.GetRequiredService<InvoiceEditorKeyboardHandler>();
            km.Register(AppInteractionState.EditingInvoice, editorHandler);
            km.Register(AppInteractionState.InlineCreatorActive, editorHandler);
            km.Register(AppInteractionState.InlinePromptActive, editorHandler);

            var masterHandler = Provider.GetRequiredService<MasterDataKeyboardHandler>();
            km.Register(AppInteractionState.EditingMasterData, masterHandler);

            var orchestrator = Provider.GetRequiredService<StartupOrchestrator>();
            cts = new CancellationTokenSource();
            progressVm = Provider.GetRequiredService<ProgressViewModel>();
            progressVm.CancelCommand = new RelayCommand(() =>
            {
                try
                {
                    cts.Cancel();
                }
                catch (ObjectDisposedException)
                {
                    // ignore; startup already completed
                }
            });
            var progress = new Progress<ProgressReport>(r =>
            {
                progressVm.GlobalProgress = r.GlobalPercent;
                progressVm.SubProgress = r.SubtaskPercent;
                progressVm.StatusMessage = r.Message;
            });

            if (await orchestrator.DatabaseEmptyAsync(cts.Token))
            {
                var optionsVm = Provider.GetRequiredService<SeedOptionsViewModel>();
                var optionsWin = Provider.GetRequiredService<SeedOptionsWindow>();
                optionsWin.DataContext = optionsVm;
                if (optionsWin.ShowDialog() == true)
                {
                    var startupWindow = Provider.GetRequiredService<StartupWindow>();
                    startupWindow.DataContext = progressVm;
                    startupWindow.Show();
                    var status = await orchestrator.SeedAsync(
                        progress,
                        cts.Token,
                        optionsVm.SupplierCount,
                        optionsVm.ProductCount,
                        optionsVm.InvoiceCount,
                        optionsVm.MinItemsPerInvoice,
                        optionsVm.MaxItemsPerInvoice,
                        true);
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
        catch (Exception ex)
        {
            ILogService log = Services != null
                ? Provider.GetRequiredService<ILogService>()
                : new Wrecept.Storage.Services.LogService();
            await log.LogError("App.OnStartup", ex);
            MessageBox.Show(
                "Váratlan hiba indításkor. Részletek a logs/startup.log fájlban.",
                "Indítási hiba",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            if (progressVm != null)
                progressVm.CancelCommand = null;
            cts?.Dispose();
        }
    }
}
