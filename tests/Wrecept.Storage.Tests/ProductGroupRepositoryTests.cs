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

public class ProductGroupRepositoryTests
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
    public async Task AddAsync_CreatesGroup()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new ProductGroupRepository(ctx);
        var group = new ProductGroup { Name = "General" };

        var id = await repo.AddAsync(group);

        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal("General", (await ctx.ProductGroups.FindAsync(id))!.Name);
    }

    [Fact]
    public async Task GetActiveAsync_ExcludesArchived()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new ProductGroupRepository(ctx);
        ctx.ProductGroups.AddRange(
            new ProductGroup { Name = "A" },
            new ProductGroup { Name = "B", IsArchived = true });
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
        var repo = new ProductGroupRepository(ctx);
        var group = new ProductGroup { Name = "Old" };
        ctx.ProductGroups.Add(group);
        await ctx.SaveChangesAsync();

        group.Name = "New";
        await repo.UpdateAsync(group);
        var stored = await ctx.ProductGroups.FindAsync(group.Id);

        Assert.Equal("New", stored!.Name);
    }
}
