using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;
using Xunit;

namespace Wrecept.Storage.Tests;

public class PaymentMethodRepositoryTests
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
    public async Task AddAsync_CreatesMethod()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new PaymentMethodRepository(ctx);
        var method = new PaymentMethod { Name = "Cash" };

        var id = await repo.AddAsync(method);

        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal("Cash", (await ctx.PaymentMethods.FindAsync(id))!.Name);
    }

    [Fact]
    public async Task GetActiveAsync_ExcludesArchived()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new PaymentMethodRepository(ctx);
        ctx.PaymentMethods.AddRange(
            new PaymentMethod { Name = "A" },
            new PaymentMethod { Name = "B", IsArchived = true });
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
        var repo = new PaymentMethodRepository(ctx);
        var method = new PaymentMethod { Name = "Card" };
        ctx.PaymentMethods.Add(method);
        await ctx.SaveChangesAsync();

        method.Name = "Transfer";
        await repo.UpdateAsync(method);
        var stored = await ctx.PaymentMethods.FindAsync(method.Id);

        Assert.Equal("Transfer", stored!.Name);
    }
}
