using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Services;

namespace Wrecept.Storage.Data;

public static class DbInitializer
{
    public static async Task EnsureCreatedAndMigratedAsync(AppDbContext db, ILogService logService, CancellationToken ct = default)
    {
        try
        {
            var pending = await db.Database.GetPendingMigrationsAsync(ct);
            if (pending.Any())
                await db.Database.MigrateAsync(ct);
        }
        catch (SqliteException ex)
        {
            await logService.LogError("Migration failed", ex);
            await db.Database.EnsureCreatedAsync(ct);
            await db.Database.MigrateAsync(ct);
        }
    }
}
