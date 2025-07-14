using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InvoiceApp.Data;
using InvoiceApp.Data.Data;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Tests;

public class AddStorageRegistrationTests
{
    [Fact]
    public async Task All_Storage_Services_Are_Resolvable()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        var userPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var settingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

        var services = new ServiceCollection();
        await services.AddStorageAsync(dbPath, userPath, settingsPath);
        using var provider = services.BuildServiceProvider();

        Type[] required =
        {
            typeof(IDbContextFactory<AppDbContext>),
            typeof(IInvoiceRepository),
            typeof(IProductRepository),
            typeof(ISupplierRepository),
            typeof(IPaymentMethodRepository),
            typeof(IProductGroupRepository),
            typeof(ITaxRateRepository),
            typeof(IUnitRepository),
            typeof(ILogService),
            typeof(IUserInfoService),
            typeof(ISettingsService),
            typeof(ISessionService),
            typeof(INumberingService),
            typeof(IBackupService),
            typeof(IDbHealthService),
            typeof(IDatabaseInitializer),
            typeof(WalPragmaInterceptor)
        };

        foreach (var type in required)
        {
            var service = provider.GetService(type);
            Assert.NotNull(service);
        }
    }
}
