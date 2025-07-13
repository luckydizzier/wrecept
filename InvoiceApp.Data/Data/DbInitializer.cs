using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using InvoiceApp.Core.Services;
using System;

namespace InvoiceApp.Data.Data;

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
                await EnsureInvoiceDueDateColumnAsync(db, ct);
                return;
            }

            await db.Database.MigrateAsync(ct);
            await EnsureInvoiceDueDateColumnAsync(db, ct);
        }
        catch (Exception ex)
        {
            await logService.LogError("Initialization failed", ex);
            throw;
        }
    }

    private static async Task EnsureInvoiceDueDateColumnAsync(AppDbContext db, CancellationToken ct)
    {
        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync(ct);
        try
        {
            await using var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = "PRAGMA table_info('Invoices')";
            await using var reader = await checkCmd.ExecuteReaderAsync(ct);
            var hasColumn = false;
            while (await reader.ReadAsync(ct))
            {
                if (reader.GetString(1).Equals("DueDate", StringComparison.OrdinalIgnoreCase))
                {
                    hasColumn = true;
                    break;
                }
            }

            if (!hasColumn)
            {
                await using var alter = conn.CreateCommand();
                alter.CommandText = "ALTER TABLE \"Invoices\" ADD COLUMN \"DueDate\" TEXT NOT NULL DEFAULT '2000-01-01'";
                await alter.ExecuteNonQueryAsync(ct);
            }
        }
        finally
        {
            await conn.CloseAsync();
        }
    }
}
