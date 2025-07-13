using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;
using Wrecept.Storage.Services;
using Xunit;

namespace Wrecept.Storage.Tests;

public class DatabaseRecoveryServiceTests
{
    private class DummyLogService : ILogService
    {
        public Task LogError(string message, Exception ex) => Task.CompletedTask;
    }

    private class TestFactory : IDbContextFactory<AppDbContext>
    {
        private readonly DbContextOptions<AppDbContext> _opts;
        public TestFactory(string db)
        {
            _opts = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={db}")
                .Options;
        }
        public AppDbContext CreateDbContext() => new AppDbContext(_opts);
        public Task<AppDbContext> CreateDbContextAsync(CancellationToken ct = default)
            => Task.FromResult(new AppDbContext(_opts));
    }

    private static async Task InitializeAsync(string db)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={db}")
            .Options;
        await using var ctx = new AppDbContext(opts);
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, new DummyLogService());
    }

    [Fact]
    public async Task CheckAndRecoverAsync_NoAction_WhenIntegrityOk()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await InitializeAsync(db);
        await using var ctx = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={db}").Options);
        var repo = new SupplierRepository(ctx);
        await repo.AddAsync(new Supplier { Name = "Test" });
        var factory = new TestFactory(db);
        var svc = new DatabaseRecoveryService(db, factory, new DummyLogService());

        await svc.CheckAndRecoverAsync();

        Assert.True(File.Exists(db));
        var backupDir = Path.Combine(Path.GetDirectoryName(db)!, "backup");
        Assert.False(Directory.Exists(backupDir));
    }

    [Fact]
    public async Task CheckAndRecoverAsync_RestoresData()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await InitializeAsync(db);
        await using (var ctx = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={db}").Options))
        {
            var repo = new SupplierRepository(ctx);
            await repo.AddAsync(new Supplier { Name = "Demo" });
        }

        // corrupt file
        using (var fs = new FileStream(db, FileMode.Open, FileAccess.Write))
        {
            fs.Seek(0, SeekOrigin.Begin);
            fs.WriteByte(0);
        }

        var factory = new TestFactory(db);
        var svc = new DatabaseRecoveryService(db, factory, new DummyLogService());

        await svc.CheckAndRecoverAsync();

        var backupDir = Path.Combine(Path.GetDirectoryName(db)!, "backup");
        Assert.True(File.Exists(Path.Combine(backupDir, $"corrupt_{DateTime.UtcNow:yyyyMMdd}.db")));

        await using var verify = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={db}").Options);
        Assert.True(await verify.Suppliers.AnyAsync());
    }
}

