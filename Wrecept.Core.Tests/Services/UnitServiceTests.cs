using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class UnitServiceTests
{
    private sealed class FakeRepo : IUnitRepository
    {
        public Unit? Added;
        public Unit? Updated;
        public Task<List<Unit>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<List<Unit>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<Guid> AddAsync(Unit unit, CancellationToken ct = default)
        {
            Added = unit;
            return Task.FromResult(Guid.NewGuid());
        }
        public Task UpdateAsync(Unit unit, CancellationToken ct = default)
        {
            Updated = unit;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task AddAsync_SetsDates()
    {
        var repo = new FakeRepo();
        var svc = new UnitService(repo);
        var unit = new Unit { Name = "kg" };
        var before = DateTime.UtcNow;

        await svc.AddAsync(unit);

        Assert.NotNull(repo.Added);
        Assert.True(repo.Added!.CreatedAt >= before);
        Assert.True(repo.Added.UpdatedAt >= before);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenNameMissing()
    {
        var repo = new FakeRepo();
        var svc = new UnitService(repo);
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new Unit()));
    }

    [Fact]
    public async Task UpdateAsync_SetsUpdatedAt()
    {
        var repo = new FakeRepo();
        var svc = new UnitService(repo);
        var unit = new Unit { Id = Guid.NewGuid(), Name = "kg" };
        var before = DateTime.UtcNow;

        await svc.UpdateAsync(unit);

        Assert.NotNull(repo.Updated);
        Assert.True(repo.Updated!.UpdatedAt >= before);
    }

    [Fact]
    public async Task UpdateAsync_AllowsArchiving()
    {
        var repo = new FakeRepo();
        var svc = new UnitService(repo);
        var unit = new Unit { Id = Guid.NewGuid(), Name = "kg", IsArchived = true };

        await svc.UpdateAsync(unit);

        Assert.True(repo.Updated?.IsArchived);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenIdInvalid()
    {
        var repo = new FakeRepo();
        var svc = new UnitService(repo);
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(new Unit { Name = "kg" }));
    }
}
