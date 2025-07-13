using System;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Services;

public class DatabaseRecoveryService : IDatabaseRecoveryService
{
    private readonly string _dbPath;
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly ILogService _log;

    public DatabaseRecoveryService(string dbPath, IDbContextFactory<AppDbContext> factory, ILogService log)
    {
        _dbPath = dbPath;
        _factory = factory;
        _log = log;
    }

    public async Task CheckAndRecoverAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Database.OpenConnectionAsync(ct);
        await using var cmd = db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "PRAGMA integrity_check;";
        var result = (string?)await cmd.ExecuteScalarAsync(ct);
        await db.Database.CloseConnectionAsync();
        if (result == "ok")
            return;

        await _log.LogError("DbRecovery", new Exception(result ?? "unknown"));
        var backupDir = Path.Combine(Path.GetDirectoryName(_dbPath)!, "backup");
        Directory.CreateDirectory(backupDir);
        var backupPath = Path.Combine(backupDir, $"corrupt_{DateTime.UtcNow:yyyyMMdd}.db");
        File.Copy(_dbPath, backupPath, true);

        // read changelog from corrupt db
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={backupPath}")
            .Options;
        await using var corruptCtx = new AppDbContext(opts);
        List<ChangeLog> logs = await corruptCtx.ChangeLogs.AsNoTracking().OrderBy(c => c.Id).ToListAsync(ct);

        File.Delete(_dbPath);
        await using var recreate = await _factory.CreateDbContextAsync(ct);
        await DbInitializer.EnsureCreatedAndMigratedAsync(recreate, _log, ct);

        foreach (var logEntry in logs)
        {
            await ApplyLogAsync(recreate, logEntry, ct);
        }
        await recreate.SaveChangesAsync(ct);
    }

    private static async Task ApplyLogAsync(AppDbContext db, ChangeLog log, CancellationToken ct)
    {
        var type = Type.GetType($"Wrecept.Core.Models.{log.Entity}");
        if (type == null)
            return;
        if (log.Operation.Equals("Insert", StringComparison.OrdinalIgnoreCase))
        {
            var obj = JsonSerializer.Deserialize(log.Data, type);
            if (obj != null)
                db.Add(obj);
        }
        else if (log.Operation.Equals("Update", StringComparison.OrdinalIgnoreCase))
        {
            var obj = JsonSerializer.Deserialize(log.Data, type);
            if (obj != null)
                db.Update(obj);
        }
        else if (log.Operation.Equals("Delete", StringComparison.OrdinalIgnoreCase))
        {
            object? key = ParseKey(log.EntityId);
            var existing = key != null ? await db.FindAsync(type, new object?[] { key }, ct) : null;
            if (existing != null)
                db.Remove(existing);
        }
    }

    private static object? ParseKey(string id)
    {
        if (Guid.TryParse(id, out var g))
            return g;
        if (int.TryParse(id, out var i))
            return i;
        return null;
    }
}

