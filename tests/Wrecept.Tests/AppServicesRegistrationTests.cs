using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf;
using Wrecept.Core.Entities;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Views.Controls;
using Wrecept.Wpf.Services;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Tests;

public class AppServicesRegistrationTests
{
    [Fact]
    public async Task ConfigureServicesAsync_RegistersAllTypes()
    {
        var method = typeof(App).GetMethod("ConfigureServicesAsync", BindingFlags.NonPublic | BindingFlags.Static)!;
        var db = Path.GetTempFileName();
        var user = Path.GetTempFileName();
        var settings = new AppSettings { DatabasePath = db, UserInfoPath = user };
        var services = new ServiceCollection();
        await (Task)method.Invoke(null, new object[] { services, settings })!;
        using var provider = services.BuildServiceProvider();

        Type[] types =
        {
            typeof(StageViewModel), typeof(InvoiceEditorViewModel), typeof(InvoiceLookupViewModel),
            typeof(ProductMasterViewModel), typeof(ProductGroupMasterViewModel), typeof(SupplierMasterViewModel),
            typeof(TaxRateMasterViewModel), typeof(PaymentMethodMasterViewModel), typeof(UnitMasterViewModel),
            typeof(UserInfoViewModel), typeof(UserInfoEditorViewModel), typeof(ScreenModeViewModel),
            typeof(AboutViewModel), typeof(PlaceholderViewModel), typeof(StatusBarViewModel),
            typeof(AppStateService), typeof(INotificationService), typeof(IInvoiceExportService),
            typeof(ScreenModeManager), typeof(FocusManager), typeof(KeyboardManager), typeof(ProgressViewModel),
            typeof(SeedOptionsViewModel), typeof(SeedOptionsWindow), typeof(StartupWindow), typeof(ScreenModeWindow),
            typeof(StartupOrchestrator), typeof(StageView), typeof(InvoiceLookupView), typeof(ProductMasterView),
            typeof(ProductGroupMasterView), typeof(SupplierMasterView), typeof(TaxRateMasterView), typeof(PaymentMethodMasterView),
            typeof(UnitMasterView), typeof(UserInfoView), typeof(UserInfoWindow), typeof(AboutView),
            typeof(PlaceholderView), typeof(Views.Controls.StatusBar), typeof(MainWindow)
        };

        foreach (var t in types)
            Assert.NotNull(provider.GetService(t));

        Assert.Equal(db, App.DbPath);
        Assert.Equal(user, App.UserInfoPath);
    }
}
