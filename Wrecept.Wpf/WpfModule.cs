using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Plugins.Abstractions;
using Wrecept.Core;
using Wrecept.Storage;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Services;
using Wrecept.Core.Services;

namespace Wrecept.Wpf;

public class WpfModule : IPlugin
{
    public async Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null)
    {
        var dbPath = context?["DbPath"] as string ?? string.Empty;
        var userInfoPath = context?["UserInfoPath"] as string ?? string.Empty;
        var settingsPath = context?["SettingsPath"] as string ?? string.Empty;

        services.AddCore();
        await services.AddStorageAsync(dbPath, userInfoPath, settingsPath);

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
        services.AddSingleton<AppStateService>(_ => new AppStateService(settingsPath.Replace("settings.json", "state.json")));
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
}
