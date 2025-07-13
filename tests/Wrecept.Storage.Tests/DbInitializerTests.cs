using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using InvoiceApp.Core.Services;
using InvoiceApp.Data.Data;
using Xunit;

namespace InvoiceApp.Data.Tests;

public class DbInitializerTests
{
    private class DummyLogService : ILogService
    {
        public Task LogError(string message, Exception ex) => Task.CompletedTask;
    }

    [Fact]
    public async Task EnsureCreatedAndMigratedAsync_CreatesDatabase()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        var log = new DummyLogService();
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={db}")
            .Options;
        await using var ctx = new AppDbContext(opts);

        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, log);

        await using var conn = new SqliteConnection($"Data Source={db}");
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "PRAGMA table_info('Invoices')";
        await using var reader = await cmd.ExecuteReaderAsync();
        var hasDueDate = false;
        while (await reader.ReadAsync())
        {
            if (reader.GetString(1).Equals("DueDate", StringComparison.OrdinalIgnoreCase))
            {
                hasDueDate = true;
                break;
            }
        }
        Assert.True(hasDueDate);
    }
}
