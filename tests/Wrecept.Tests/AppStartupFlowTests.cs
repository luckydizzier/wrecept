using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Wpf;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class AppStartupFlowTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    private static void InvokeStartup(App app)
    {
        var m = typeof(App).GetMethod("OnStartup", BindingFlags.Instance | BindingFlags.NonPublic)!;
        m.Invoke(app, new object[] { new StartupEventArgs(Array.Empty<string>()) });
    }

    private static ServiceProvider BuildProvider(string dir, bool includeOptionsVm)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILogService, NullLogService>();
        services.AddSingleton(new AppStateService(Path.Combine(dir, "state.json")));
        services.AddSingleton<StartupOrchestrator>();
        services.AddTransient<ProgressViewModel>();
        if (includeOptionsVm)
            services.AddTransient<SeedOptionsViewModel>();
        services.AddTransient<SeedOptionsWindow>();
        services.AddTransient<StartupWindow>();
        services.AddTransient<ScreenModeManager>(_ => new ScreenModeManager(new FakeSettingsService()));
        services.AddTransient(sp => (StageView)FormatterServices.GetUninitializedObject(typeof(StageView)));
        services.AddTransient<MainWindow>();
        return services.BuildServiceProvider();
    }

    private class FakeSettingsService : ISettingsService
    {
        public Task<AppSettings> LoadAsync() => Task.FromResult(new AppSettings());
        public Task SaveAsync(AppSettings settings) => Task.CompletedTask;
    }

    [StaFact]
    public void OnStartup_Seeds_WhenDatabaseEmpty()
    {
        EnsureApp();
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        App.DbPath = Path.Combine(dir, "app.db");
        App.UserInfoPath = Path.Combine(dir, "user.json");
        App.SettingsPath = Path.Combine(dir, "settings.json");
        App.StatePath = Path.Combine(dir, "state.json");
        var provider = BuildProvider(dir, includeOptionsVm: true);
        var optionsWindow = provider.GetRequiredService<SeedOptionsWindow>();
        optionsWindow.Loaded += (_, _) =>
        {
            if (optionsWindow.DataContext is SeedOptionsViewModel vm)
            {
                vm.SupplierCount = 1;
                vm.ProductCount = 1;
                vm.InvoiceCount = 1;
                vm.MinItemsPerInvoice = 1;
                vm.MaxItemsPerInvoice = 1;
            }
            optionsWindow.DialogResult = true;
        };
        App.Services = provider;
        var app = new App();

        InvokeStartup(app);

        Assert.True(File.Exists(App.DbPath));
        Assert.IsType<MainWindow>(Application.Current!.MainWindow);
    }

    private class RecordingLog : ILogService
    {
        public bool Called;
        public Task LogError(string message, Exception ex) { Called = true; return Task.CompletedTask; }
    }

    [StaFact]
    public void OnStartup_LogsError_WhenServiceMissing()
    {
        EnsureApp();
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        App.DbPath = Path.Combine(dir, "app.db");
        App.UserInfoPath = Path.Combine(dir, "user.json");
        App.SettingsPath = Path.Combine(dir, "settings.json");
        App.StatePath = Path.Combine(dir, "state.json");
        var log = new RecordingLog();
        var services = new ServiceCollection();
        services.AddSingleton<ILogService>(log);
        services.AddSingleton(new AppStateService(App.StatePath));
        services.AddSingleton<StartupOrchestrator>();
        services.AddTransient<ProgressViewModel>();
        services.AddTransient<SeedOptionsWindow>();
        services.AddTransient<StartupWindow>();
        services.AddTransient(sp => (StageView)FormatterServices.GetUninitializedObject(typeof(StageView)));
        services.AddTransient<ScreenModeManager>(_ => new ScreenModeManager(new FakeSettingsService()));
        services.AddTransient<MainWindow>();
        App.Services = services.BuildServiceProvider();
        var app = new App();

        InvokeStartup(app);

        Assert.True(log.Called);
    }
}
