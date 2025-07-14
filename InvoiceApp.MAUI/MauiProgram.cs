using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using Microsoft.Maui;
using Microsoft.Maui.Storage;
using InvoiceApp.Core;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.MAUI.Views;
using InvoiceApp.MAUI.Services;
using InvoiceApp.MAUI.Views.Dialogs;

namespace InvoiceApp.MAUI;

public static class MauiProgram
{
    public static IServiceProvider? Services { get; private set; }
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        ConfigureServices(builder.Services);

        var app = builder.Build();
        Services = app.Services;
        return app;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var appDir = FileSystem.AppDataDirectory;
        var settingsPath = Path.Combine(appDir, "settings.json");
        services.AddCore();
        services.AddStorageAsync(Path.Combine(appDir, "app.db"), Path.Combine(appDir, "user.json"), settingsPath).GetAwaiter().GetResult();
        services.AddTransient<ISetupFlow, SetupFlow>();
        services.AddTransient<Dialogs.SetupPage>();
        services.AddTransient<Dialogs.UserInfoEditorPage>();
        services.AddTransient<Dialogs.SeedOptionsPage>();
        services.AddTransient<SeedOptionsViewModel>();
        services.AddTransient<UserInfoEditorViewModel>();
        services.AddSingleton<AppStateService>(_ => new AppStateService(Path.Combine(appDir, "state.json")));
        services.AddSingleton<KeyboardManager>();
        services.AddSingleton<FocusManager>();
        services.AddSingleton<StageViewModel>();
        services.AddSingleton<StatusBarViewModel>();
        services.AddSingleton<InvoiceEditorViewModel>();
        services.AddTransient<ProductMasterViewModel>();
        services.AddTransient<SupplierMasterViewModel>();
        services.AddSingleton<ScreenModeManager>();
        services.AddTransient<StageMenuHandler>();
        services.AddTransient<StageMenuKeyboardHandler>();
        services.AddTransient<MasterDataKeyboardHandler>();
        services.AddTransient<InvoiceEditorKeyboardHandler>();
        services.AddTransient<InvoiceLookupKeyboardHandler>();
        services.AddTransient<StartupOrchestrator>();
        services.AddTransient<MainPage>();
        services.AddTransient<Views.StageView>();
    }
}
