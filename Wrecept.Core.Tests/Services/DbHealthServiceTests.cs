using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Storage.Data;
using Wrecept.Storage.Services;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class DbHealthServiceTests
{
    private class FailFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext() => throw new InvalidOperationException();
        public ValueTask<AppDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
            => throw new InvalidOperationException();
    }

    private class LogSpy : ILogService
    {
        public Exception? Last;
        public Task LogError(string message, Exception ex)
        {
            Last = ex;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task CheckAsync_ReturnsTrue_ForValidDb()
    {
        var services = new ServiceCollection();
        services.AddDbContextFactory<AppDbContext>(o => o.UseSqlite("Data Source=:memory:"));
        await using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using (var ctx = await factory.CreateDbContextAsync())
        {
            await ctx.Database.EnsureCreatedAsync();
        }
        var svc = new DbHealthService(factory, new NullLogService());
        var ok = await svc.CheckAsync();
        Assert.True(ok);
    }

    [Fact]
    public async Task CheckAsync_LogsAndReturnsFalse_OnException()
    {
        var log = new LogSpy();
        var svc = new DbHealthService(new FailFactory(), log);
        var ok = await svc.CheckAsync();
        Assert.False(ok);
        Assert.NotNull(log.Last);
    }
}
