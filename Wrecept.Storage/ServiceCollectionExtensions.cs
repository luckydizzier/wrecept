using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;

namespace Wrecept.Storage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string dbPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(dbPath);

        services.AddDbContext<AppDbContext>(o => o.UseSqlite($"Data Source={dbPath}"));
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        services.AddScoped<ITaxRateRepository, TaxRateRepository>();

        using var provider = services.BuildServiceProvider();
        var ctx = provider.GetRequiredService<AppDbContext>();
        ctx.Database.EnsureCreated();

        return services;
    }
}
