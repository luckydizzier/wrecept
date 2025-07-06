using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Xunit;

namespace Wrecept.Storage.Tests;

public class DataSeederTests
{
    private class DummyLogService : ILogService
    {
        public Task LogError(string message, Exception ex) => Task.CompletedTask;
    }

    [Fact]
    public async Task SeedSampleDataAsync_PopulatesTables()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        var log = new DummyLogService();

        var status = await DataSeeder.SeedSampleDataAsync(db, log);

        Assert.Equal(SeedStatus.Seeded, status);
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={db}")
            .Options;
        await using var ctx = new AppDbContext(opts);
        Assert.True(await ctx.Invoices.AnyAsync());
        Assert.True(await ctx.Products.AnyAsync());
        Assert.True(await ctx.Suppliers.AnyAsync());
    }

    [Fact]
    public async Task IsDatabaseEmptyAsync_ReturnsTrue_ForNewDatabase()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        var log = new DummyLogService();

        var empty = await DataSeeder.IsDatabaseEmptyAsync(db, log);

        Assert.True(empty);
    }
}
