using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace InvoiceApp.Data.Data;

public class WalPragmaInterceptor : DbConnectionInterceptor
{
    private const string Sql = "PRAGMA journal_mode=WAL";

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        if (connection is SqliteConnection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql;
            await cmd.ExecuteScalarAsync(cancellationToken);
        }
    }

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        if (connection is SqliteConnection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql;
            cmd.ExecuteScalar();
        }
    }
}
