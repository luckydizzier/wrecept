using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class UnitServiceTests
{
    private class FakeRepo : IUnitRepository
    {
        public Unit? Added;
        public Unit? Updated;
        public Task<List<Unit>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<List<Unit>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<Guid> AddAsync(Unit unit, CancellationToken ct = default) { Added = unit; return Task.FromResult(Guid.NewGuid()); }
        public Task UpdateAsync(Unit unit, CancellationToken ct = default) { Updated = unit; return Task.CompletedTask; }
    }

    [Fact]
    public async Task AddAsync_SetsTimestamps()
    {
        var repo = new FakeRepo();
        var svc = new UnitService(repo);
        var unit = new Unit { Name = "u", CreatedAt = DateTime.MinValue, UpdatedAt = DateTime.MinValue };

        await svc.AddAsync(unit);

        Assert.Equal(unit, repo.Added);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.UpdatedAt);
    }

    [Fact]
    public async Task AddAsync_InvalidName_Throws()
    {
        var svc = new UnitService(new FakeRepo());
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new Unit()));
    }

    [Fact]
    public async Task UpdateAsync_SetsTimestamp()
    {
        var repo = new FakeRepo();
        var svc = new UnitService(repo);
        var unit = new Unit { Id = Guid.NewGuid(), Name = "u", UpdatedAt = DateTime.MinValue };

        await svc.UpdateAsync(unit);

        Assert.Equal(unit, repo.Updated);
        Assert.NotEqual(DateTime.MinValue, repo.Updated!.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_Throws()
    {
        var svc = new UnitService(new FakeRepo());
        var unit = new Unit { Id = Guid.Empty, Name = "u" };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(unit));
    }
}
