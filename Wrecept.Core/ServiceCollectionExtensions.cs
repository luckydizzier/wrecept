using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;

namespace Wrecept.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IProductGroupService, ProductGroupService>();
        services.AddScoped<ITaxRateService, TaxRateService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddSingleton<ILogService, NullLogService>();
        return services;
    }
}
