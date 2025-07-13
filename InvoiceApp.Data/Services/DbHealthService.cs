using Microsoft.EntityFrameworkCore;
using InvoiceApp.Core.Services;
using InvoiceApp.Data.Data;

namespace InvoiceApp.Data.Services;

public class DbHealthService : IDbHealthService
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly ILogService _log;

    public DbHealthService(IDbContextFactory<AppDbContext> factory, ILogService log)
    {
        _factory = factory;
        _log = log;
    }

    public async Task<bool> CheckAsync(CancellationToken ct = default)
    {
        try
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            await db.Database.OpenConnectionAsync(ct);
            await using var cmd = db.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "PRAGMA integrity_check;";
            var result = (string?)await cmd.ExecuteScalarAsync(ct);
            await db.Database.CloseConnectionAsync();
            if (result != "ok")
            {
                await _log.LogError("DbHealth", new Exception(result ?? "unknown"));
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            await _log.LogError("DbHealth", ex);
            return false;
        }
    }
}
