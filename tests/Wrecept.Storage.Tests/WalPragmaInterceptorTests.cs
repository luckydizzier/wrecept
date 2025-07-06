using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Storage.Data;
using Xunit;

namespace Wrecept.Storage.Tests;

public class WalPragmaInterceptorTests
{
    [Fact]
    public async Task ConnectionOpenedAsync_SetsJournalModeToWal()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var connection = new SqliteConnection($"Data Source={path}");
        await connection.OpenAsync();
        await using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "PRAGMA journal_mode";
            var initial = (string)await cmd.ExecuteScalarAsync();
            Assert.NotEqual("wal", initial.ToLowerInvariant());
        }

        var interceptor = new WalPragmaInterceptor();
        await interceptor.ConnectionOpenedAsync(connection, null!);

        await using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "PRAGMA journal_mode";
            var mode = (string)await cmd.ExecuteScalarAsync();
            Assert.Equal("wal", mode.ToLowerInvariant());
        }
    }
}
