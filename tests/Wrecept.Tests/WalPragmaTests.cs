using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Storage;
using Wrecept.Storage.Data;
using Wrecept.Core.Repositories;
using Xunit;
using System;
using System.IO;

namespace Wrecept.Tests;

public class WalPragmaTests
{
    [Fact]
    public async Task JournalMode_Is_Wal()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        var userPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var settingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

        var services = new ServiceCollection();
        await services.AddStorageAsync(dbPath, userPath, settingsPath);
        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var ctx = await factory.CreateDbContextAsync();
        await ctx.Database.OpenConnectionAsync();
        await using var cmd = ctx.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "PRAGMA journal_mode";
        var mode = (string)await cmd.ExecuteScalarAsync();
        await ctx.Database.CloseConnectionAsync();

        Assert.Equal("wal", mode.ToLowerInvariant());
    }

    [Fact]
    public async Task EmptyDbPath_ResolvesServicesAndCreatesFile()
    {
        var userPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var settingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

        var services = new ServiceCollection();
        await services.AddStorageAsync(string.Empty, userPath, settingsPath);
        using var provider = services.BuildServiceProvider();

        var interceptor = provider.GetRequiredService<WalPragmaInterceptor>();
        var repo = provider.GetRequiredService<IInvoiceRepository>();

        var expected = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Wrecept", "app.db");

        Assert.True(File.Exists(expected));

        File.Delete(expected);
    }
}
