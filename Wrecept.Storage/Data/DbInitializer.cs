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
            await db.Database.EnsureCreatedAsync(ct);
            await db.Database.MigrateAsync(ct);
        }
        catch (SqliteException ex)
        {
            await logService.LogError("Migration failed", ex);
            await db.Database.EnsureCreatedAsync(ct);
            await db.Database.MigrateAsync(ct);
        }
        catch (Exception ex)
        {
            await logService.LogError("Initialization failed", ex);
            await db.Database.EnsureCreatedAsync(ct);
            try
            {
                await db.Database.MigrateAsync(ct);
            }
            catch (Exception inner)
            {
                await logService.LogError("Second migration attempt failed", inner);
                throw;
            }
        }
    }
}
