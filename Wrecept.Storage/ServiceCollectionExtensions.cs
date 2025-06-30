using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;
using Wrecept.Storage.Services;

namespace Wrecept.Storage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string dbPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(dbPath);

        services.AddDbContext<AppDbContext>(o => o.UseSqlite($"Data Source={dbPath}"));
        services.AddDbContextFactory<AppDbContext>(o => o.UseSqlite($"Data Source={dbPath}"));
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        services.AddScoped<ITaxRateRepository, TaxRateRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddSingleton<ILogService, LogService>();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        var logger = provider.GetRequiredService<ILogService>();
        using var ctx = factory.CreateDbContext();
        DbInitializer.EnsureCreatedAndMigratedAsync(ctx, logger).GetAwaiter().GetResult();

        return services;
    }
}
