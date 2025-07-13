using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Maui.Storage;

namespace InvoiceApp.Data;

public static class ServiceCollectionExtensions
{
    public static Task AddStorageAsync(this IServiceCollection services, string dbPath, string userInfoPath, string settingsPath)
    {
        if (string.IsNullOrWhiteSpace(dbPath))
        {
            var appDir = FileSystem.AppDataDirectory;
            Directory.CreateDirectory(appDir);
            dbPath = Path.Combine(appDir, "app.db");
        }

        services.AddSingleton<Data.WalPragmaInterceptor>();
        services.AddDbContext<Data.AppDbContext>((sp, o) =>
            o.UseSqlite($"Data Source={dbPath}")
             .AddInterceptors(sp.GetRequiredService<Data.WalPragmaInterceptor>()));
        services.AddDbContextFactory<Data.AppDbContext>((sp, o) =>
            o.UseSqlite($"Data Source={dbPath}")
             .AddInterceptors(sp.GetRequiredService<Data.WalPragmaInterceptor>()));

        return Task.CompletedTask;
    }
}
