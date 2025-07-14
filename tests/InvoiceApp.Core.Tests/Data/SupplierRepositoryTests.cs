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

public class SupplierRepositoryTests
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
    public async Task AddAsync_CreatesSupplier()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new SupplierRepository(ctx);
        var supplier = new Supplier { Name = "Acme" };

        var id = await repo.AddAsync(supplier);

        Assert.True(id > 0);
        Assert.Equal("Acme", (await ctx.Suppliers.FindAsync(id))!.Name);
    }

    [Fact]
    public async Task GetActiveAsync_ExcludesArchived()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new SupplierRepository(ctx);
        ctx.Suppliers.AddRange(
            new Supplier { Name = "A" },
            new Supplier { Name = "B", IsArchived = true });
        await ctx.SaveChangesAsync();

        var list = await repo.GetActiveAsync();

        Assert.Single(list);
        Assert.Equal("A", list[0].Name);
    }
}
