using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Services;

namespace Wrecept.Core.Tests;

public class SuggestionIndexServiceTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;
        var ctx = new AppDbContext(options);
        ctx.Database.OpenConnection();
        ctx.Database.EnsureCreated();
        return ctx;
    }

    [Fact]
    public async Task AddHistoryEntry_UpdatesPredictions()
    {
        using var ctx = CreateContext();
        var svc = new SuggestionIndexService(ctx);
        await svc.AddHistoryEntryAsync("Coffee");
        await svc.AddHistoryEntryAsync("Coffee");
        await svc.AddHistoryEntryAsync("Tea");

        var preds = await svc.GetPredictionsAsync("C");
        Assert.Contains("Coffee", preds);
    }
}
