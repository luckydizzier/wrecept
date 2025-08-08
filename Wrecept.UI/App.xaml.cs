using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Windows;
using Wrecept.Core.Data;
using Wrecept.Core.Orchestration;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.UI.ViewModels;
using Wrecept.UI.Views;

namespace Wrecept.UI;

public partial class App : Application
{
    private readonly IHost _host;

    public static IServiceProvider ServiceProvider => ((App)Current)._host.Services;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile("wrecept.json", optional: false, reloadOnChange: true);
            })
            .UseSerilog((context, services, loggerConfiguration) =>
            {
                var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "wrecept-.log");
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    var dbRelativePath = context.Configuration["DatabasePath"] ?? "Data/wrecept.db";
                    var dbFullPath = Path.Combine(AppContext.BaseDirectory, dbRelativePath);
                    var dir = Path.GetDirectoryName(dbFullPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    options.UseSqlite($"Data Source={dbFullPath}");
                });
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IInvoiceService, InvoiceService>();
                services.AddSingleton<ISettingsService, SettingsService>();
                services.AddScoped<IDemoDataService, DemoDataService>();
                services.AddSingleton<StartupOrchestrator>();
                services.AddSingleton<InvoiceEditorViewModel>();
                services.AddTransient<InvoiceEditorView>();
                services.AddSingleton<StocksViewModel>();
                services.AddTransient<StocksView>();
                services.AddSingleton<ContactsViewModel>();
                services.AddTransient<ContactsView>();
                services.AddSingleton<ListsViewModel>();
                services.AddTransient<ListsView>();
                services.AddSingleton<MaintenanceViewModel>();
                services.AddTransient<MaintenanceView>();
                services.AddSingleton<ThemeEditorViewModel>();
                services.AddTransient<ThemeEditorView>();
                services.AddSingleton<IExportService, ExportService>();
                services.AddSingleton<MainViewModel>();
                services.AddTransient<MainView>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var orchestrator = _host.Services.GetRequiredService<StartupOrchestrator>();
        await orchestrator.InitializeAsync();

        var settingsService = _host.Services.GetRequiredService<ISettingsService>();
        var settings = await settingsService.LoadAsync();
        ApplyTheme(settings.Theme);
        settingsService.SettingsChanged += (_, s) => ApplyTheme(s.Theme);

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    private void ApplyTheme(string theme)
    {
        Resources.MergedDictionaries.Clear();
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"Themes/{theme}Theme.xaml", UriKind.Relative)
        });
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
