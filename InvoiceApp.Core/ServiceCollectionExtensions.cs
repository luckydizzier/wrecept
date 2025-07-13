using Microsoft.Extensions.DependencyInjection;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<InvoiceCalculator>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IProductGroupService, ProductGroupService>();
        services.AddScoped<ITaxRateService, TaxRateService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<IUnitService, UnitService>();
        services.AddSingleton<ILogService, NullLogService>();
        services.AddSingleton<INumberingService, NullNumberingService>();
        return services;
    }
}
