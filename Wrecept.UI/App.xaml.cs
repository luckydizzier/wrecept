using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Windows;
using Wrecept.Core.Data;
using Wrecept.Core.Orchestration;
using Wrecept.Core.Services;
using Wrecept.UI.ViewModels;
using Wrecept.UI.Views;

namespace Wrecept.UI;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile("wrecept.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, context.Configuration["DatabasePath"]!)}"));
                services.AddScoped<IDemoDataService, DemoDataService>();
                services.AddSingleton<StartupOrchestrator>();
                services.AddSingleton<InvoiceEditorViewModel>();
                services.AddTransient<InvoiceEditorView>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var orchestrator = _host.Services.GetRequiredService<StartupOrchestrator>();
        await orchestrator.InitializeAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _host.Services.GetRequiredService<InvoiceEditorViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}
