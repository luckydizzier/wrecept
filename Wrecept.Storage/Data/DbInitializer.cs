using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Wrecept.Core.Services;

namespace Wrecept.Storage.Data;

public static class DbInitializer
{
    public static async Task EnsureCreatedAndMigratedAsync(AppDbContext db, ILogService logService, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(logService);

        try
        {
            var creator = db.GetService<IRelationalDatabaseCreator>();

            if (!await creator.ExistsAsync(ct))
            {
                await db.Database.MigrateAsync(ct);
                return;
            }

            await db.Database.MigrateAsync(ct);
        }
        catch (Exception ex)
        {
            await logService.LogError("Initialization failed", ex);
            throw;
        }
    }
}
