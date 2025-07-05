using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Storage;
using Wrecept.Storage.Data;
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
}
