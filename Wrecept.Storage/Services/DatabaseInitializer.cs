using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly ILogService _log;
    private readonly IDatabaseRecoveryService _recovery;

    public DatabaseInitializer(IDbContextFactory<AppDbContext> factory, ILogService log, IDatabaseRecoveryService recovery)
    {
        _factory = factory;
        _log = log;
        _recovery = recovery;
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, _log, ct);
        await ctx.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL", ct);
        await _recovery.CheckAndRecoverAsync(ct);
    }
}
