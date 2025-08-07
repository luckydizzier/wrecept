using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Data;
using Wrecept.Core.Services;

namespace Wrecept.Core.Orchestration;

public class StartupOrchestrator
{
    private readonly IServiceProvider _serviceProvider;

    public StartupOrchestrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();

        if (!await dbContext.Invoices.AnyAsync())
        {
            var demoService = _serviceProvider.GetRequiredService<IDemoDataService>();
            await demoService.SeedAsync();
        }
    }
}
