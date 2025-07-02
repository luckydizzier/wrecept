using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class SupplierServiceTests
{
    private sealed class FakeRepo : ISupplierRepository
    {
        public Supplier? Added;
        public Supplier? Updated;
        public Task<List<Supplier>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<List<Supplier>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<int> AddAsync(Supplier supplier, CancellationToken ct = default)
        {
            Added = supplier;
            return Task.FromResult(1);
        }
        public Task UpdateAsync(Supplier supplier, CancellationToken ct = default)
        {
            Updated = supplier;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task AddAsync_SetsDates()
    {
        var repo = new FakeRepo();
        var svc = new SupplierService(repo);
        var supplier = new Supplier { Name = "Supp" };
        var before = DateTime.UtcNow;

        await svc.AddAsync(supplier);

        Assert.NotNull(repo.Added);
        Assert.True(repo.Added!.CreatedAt >= before);
        Assert.True(repo.Added.UpdatedAt >= before);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenNameMissing()
    {
        var repo = new FakeRepo();
        var svc = new SupplierService(repo);
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new Supplier()));
    }

    [Fact]
    public async Task UpdateAsync_SetsUpdatedAt()
    {
        var repo = new FakeRepo();
        var svc = new SupplierService(repo);
        var supplier = new Supplier { Id = 1, Name = "Supp" };
        var before = DateTime.UtcNow;

        await svc.UpdateAsync(supplier);

        Assert.NotNull(repo.Updated);
        Assert.True(repo.Updated!.UpdatedAt >= before);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenIdInvalid()
    {
        var repo = new FakeRepo();
        var svc = new SupplierService(repo);
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(new Supplier { Name = "Supp" }));
    }
}
