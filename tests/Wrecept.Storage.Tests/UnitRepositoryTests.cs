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

public class UnitRepositoryTests
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
    public async Task AddAsync_CreatesUnit()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new UnitRepository(ctx);
        var unit = new Unit { Code = "KG", Name = "kg" };

        var id = await repo.AddAsync(unit);

        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal("kg", (await ctx.Units.FindAsync(id))!.Name);
    }

    [Fact]
    public async Task GetActiveAsync_ExcludesArchived()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new UnitRepository(ctx);
        ctx.Units.AddRange(
            new Unit { Code = "DB", Name = "db" },
            new Unit { Code = "KG", Name = "kg", IsArchived = true });
        await ctx.SaveChangesAsync();

        var list = await repo.GetActiveAsync();

        Assert.Single(list);
        Assert.Equal("db", list[0].Name);
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new UnitRepository(ctx);
        var unit = new Unit { Code = "PC", Name = "pcs" };
        ctx.Units.Add(unit);
        await ctx.SaveChangesAsync();

        unit.Name = "piece";
        await repo.UpdateAsync(unit);
        var stored = await ctx.Units.FindAsync(unit.Id);

        Assert.Equal("piece", stored!.Name);
    }
}
