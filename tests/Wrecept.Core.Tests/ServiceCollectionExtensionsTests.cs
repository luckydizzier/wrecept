using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Wrecept.Core;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCore_RegistersDependencies()
    {
        var services = new ServiceCollection();
        services.AddCore();

        Assert.Contains(services, d => d.ServiceType == typeof(IProductService) && d.ImplementationType == typeof(ProductService));
        Assert.Contains(services, d => d.ServiceType == typeof(IInvoiceService) && d.ImplementationType == typeof(InvoiceService));
        Assert.Contains(services, d => d.ServiceType == typeof(InvoiceCalculator) && d.ImplementationType == typeof(InvoiceCalculator));
        Assert.Contains(services, d => d.ServiceType == typeof(ISupplierService) && d.ImplementationType == typeof(SupplierService));
        Assert.Contains(services, d => d.ServiceType == typeof(IProductGroupService) && d.ImplementationType == typeof(ProductGroupService));
        Assert.Contains(services, d => d.ServiceType == typeof(ITaxRateService) && d.ImplementationType == typeof(TaxRateService));
        Assert.Contains(services, d => d.ServiceType == typeof(IPaymentMethodService) && d.ImplementationType == typeof(PaymentMethodService));
        Assert.Contains(services, d => d.ServiceType == typeof(IUnitService) && d.ImplementationType == typeof(UnitService));

        var numDesc = services.First(d => d.ServiceType == typeof(INumberingService));
        var logDesc = services.First(d => d.ServiceType == typeof(ILogService));
        Assert.Equal(ServiceLifetime.Singleton, numDesc.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, logDesc.Lifetime);
        Assert.Equal(typeof(NullNumberingService), numDesc.ImplementationType);
        Assert.Equal(typeof(NullLogService), logDesc.ImplementationType);
    }
}
