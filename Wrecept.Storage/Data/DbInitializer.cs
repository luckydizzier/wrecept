using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Wrecept.Storage.Data;

public static class DbInitializer
{
    public static async Task EnsureCreatedAndMigratedAsync(AppDbContext db, CancellationToken ct = default)
    {
        try
        {
            var pending = await db.Database.GetPendingMigrationsAsync(ct);
            if (pending.Any())
                await db.Database.MigrateAsync(ct);
        }
        catch (SqliteException)
        {
            await db.Database.EnsureCreatedAsync(ct);
            await db.Database.MigrateAsync(ct);
        }
    }
}
