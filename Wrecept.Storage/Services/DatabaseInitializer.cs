using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly ILogService _log;

    public DatabaseInitializer(IDbContextFactory<AppDbContext> factory, ILogService log)
    {
        _factory = factory;
        _log = log;
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, _log, ct);
        await ctx.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL", ct);
    }
}
