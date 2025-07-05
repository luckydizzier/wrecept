using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;
using Wrecept.Storage.Services;

namespace Wrecept.Storage;

public static class ServiceCollectionExtensions
{
    public static async Task AddStorageAsync(this IServiceCollection services, string dbPath, string userInfoPath, string settingsPath)
    {
        if (string.IsNullOrWhiteSpace(dbPath))
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dataDir = Path.Combine(appData, "Wrecept");
            Directory.CreateDirectory(dataDir);
            dbPath = Path.Combine(dataDir, "app.db");
        }

        services.AddSingleton<WalPragmaInterceptor>();

        services.AddDbContext<AppDbContext>((sp, o) =>
            o.UseSqlite($"Data Source={dbPath}")
             .AddInterceptors(sp.GetRequiredService<WalPragmaInterceptor>()));

        services.AddDbContextFactory<AppDbContext>((sp, o) =>
            o.UseSqlite($"Data Source={dbPath}")
             .AddInterceptors(sp.GetRequiredService<WalPragmaInterceptor>()));
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        services.AddScoped<ITaxRateRepository, TaxRateRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IUserInfoService>(_ => new UserInfoService(userInfoPath));
        services.AddSingleton<ISettingsService>(_ => new SettingsService(settingsPath));
        var sessionPath = Path.Combine(Path.GetDirectoryName(settingsPath)!, "session.json");
        services.AddSingleton<ISessionService>(_ => new SessionService(sessionPath));
        services.AddScoped<IDbHealthService, DbHealthService>();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        var logger = provider.GetRequiredService<ILogService>();
        await using var ctx = factory.CreateDbContext();
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, logger);
        await ctx.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL");
    }
}
