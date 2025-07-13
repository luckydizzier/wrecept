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

public class ProductRepositoryTests
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
    public async Task AddAsync_CreatesProduct()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new ProductRepository(ctx);
        var tax = new TaxRate { Id = Guid.NewGuid(), Code = "T1", Name = "Tax", Percentage = 10m, EffectiveFrom = DateTime.UtcNow };
        var unit = new Unit { Id = Guid.NewGuid(), Code = "DB", Name = "db" };
        var group = new ProductGroup { Id = Guid.NewGuid(), Name = "Group" };
        ctx.TaxRates.Add(tax);
        ctx.Units.Add(unit);
        ctx.ProductGroups.Add(group);
        await ctx.SaveChangesAsync();
        var product = new Product { Name = "Item", Net = 100m, Gross = 110m, TaxRateId = tax.Id, UnitId = unit.Id, ProductGroupId = group.Id };

        var id = await repo.AddAsync(product);

        Assert.True(id > 0);
        Assert.Equal("Item", (await ctx.Products.FindAsync(id))!.Name);
    }

    [Fact]
    public async Task GetActiveAsync_ExcludesArchived()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new ProductRepository(ctx);
        var tax = new TaxRate { Id = Guid.NewGuid(), Code = "T", Name = "T", Percentage = 5m, EffectiveFrom = DateTime.UtcNow };
        var unit = new Unit { Id = Guid.NewGuid(), Code = "U", Name = "u" };
        var group = new ProductGroup { Id = Guid.NewGuid(), Name = "g" };
        ctx.TaxRates.Add(tax);
        ctx.Units.Add(unit);
        ctx.ProductGroups.Add(group);
        ctx.Products.AddRange(
            new Product { Name = "A", TaxRateId = tax.Id, UnitId = unit.Id, ProductGroupId = group.Id },
            new Product { Name = "B", IsArchived = true, TaxRateId = tax.Id, UnitId = unit.Id, ProductGroupId = group.Id });
        await ctx.SaveChangesAsync();

        var list = await repo.GetActiveAsync();

        Assert.Single(list);
        Assert.Equal("A", list[0].Name);
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new ProductRepository(ctx);
        var tax = new TaxRate { Id = Guid.NewGuid(), Code = "T2", Name = "T", Percentage = 5m, EffectiveFrom = DateTime.UtcNow };
        var unit = new Unit { Id = Guid.NewGuid(), Code = "U2", Name = "u" };
        var group = new ProductGroup { Id = Guid.NewGuid(), Name = "g" };
        ctx.TaxRates.Add(tax);
        ctx.Units.Add(unit);
        ctx.ProductGroups.Add(group);
        var product = new Product { Name = "Old", Net = 1m, Gross = 1.1m, TaxRateId = tax.Id, UnitId = unit.Id, ProductGroupId = group.Id };
        ctx.Products.Add(product);
        await ctx.SaveChangesAsync();

        product.Name = "New";
        await repo.UpdateAsync(product);
        var stored = await ctx.Products.FindAsync(product.Id);

        Assert.Equal("New", stored!.Name);
    }
}
