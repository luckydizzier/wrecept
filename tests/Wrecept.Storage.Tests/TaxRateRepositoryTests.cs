using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;
using InvoiceApp.Data.Data;
using InvoiceApp.Data.Repositories;
using Xunit;

namespace InvoiceApp.Data.Tests;

public class TaxRateRepositoryTests
{
    private class DummyLogService : ILogService
    {
        public Task LogError(string message, Exception ex) => Task.CompletedTask;
    }

    private static async Task<AppDbContext> CreateContextAsync(string db)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={db}")
            .Options;
        var ctx = new AppDbContext(opts);
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, new DummyLogService());
        return ctx;
    }

    [Fact]
    public async Task AddAsync_CreatesTaxRate()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new TaxRateRepository(ctx);
        var tax = new TaxRate { Code = "A27", Name = "A", Percentage = 27m, EffectiveFrom = DateTime.UtcNow };

        var id = await repo.AddAsync(tax);

        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal("A", (await ctx.TaxRates.FindAsync(id))!.Name);
    }

    [Fact]
    public async Task GetActiveAsync_RespectsDate()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new TaxRateRepository(ctx);
        var now = DateTime.UtcNow;
        ctx.TaxRates.AddRange(
            new TaxRate { Code = "O", Name = "Old", Percentage = 5m, EffectiveFrom = now.AddMonths(-2), EffectiveTo = now.AddMonths(-1) },
            new TaxRate { Code = "C", Name = "Current", Percentage = 27m, EffectiveFrom = now.AddMonths(-1) });
        await ctx.SaveChangesAsync();

        var list = await repo.GetActiveAsync(now);

        Assert.Single(list);
        Assert.Equal("Current", list[0].Name);
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new TaxRateRepository(ctx);
        var tax = new TaxRate { Code = "A0", Name = "Old", Percentage = 5m, EffectiveFrom = DateTime.UtcNow };
        ctx.TaxRates.Add(tax);
        await ctx.SaveChangesAsync();

        tax.Name = "New";
        await repo.UpdateAsync(tax);
        var stored = await ctx.TaxRates.FindAsync(tax.Id);

        Assert.Equal("New", stored!.Name);
    }
}
