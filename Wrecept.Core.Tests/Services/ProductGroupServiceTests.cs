using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

public class ProductGroupServiceTests
{
    private sealed class FakeRepo : IProductGroupRepository
    {
        public ProductGroup? Added;
        public ProductGroup? Updated;
        public Task<List<ProductGroup>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<List<ProductGroup>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<Guid> AddAsync(ProductGroup group, CancellationToken ct = default)
        {
            Added = group;
            return Task.FromResult(Guid.NewGuid());
        }
        public Task UpdateAsync(ProductGroup group, CancellationToken ct = default)
        {
            Updated = group;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task AddAsync_Throws_WhenNameMissing()
    {
        var repo = new FakeRepo();
        var svc = new ProductGroupService(repo);
        var group = new ProductGroup { Name = "" };

        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(group));
    }

    [Fact]
    public async Task AddAsync_CallsRepository()
    {
        var repo = new FakeRepo();
        var svc = new ProductGroupService(repo);
        var group = new ProductGroup { Name = "Group" };

        await svc.AddAsync(group);

        Assert.NotNull(repo.Added);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenIdInvalid()
    {
        var repo = new FakeRepo();
        var svc = new ProductGroupService(repo);
        var group = new ProductGroup { Name = "Group" };

        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(group));
    }

    [Fact]
    public async Task UpdateAsync_CallsRepository()
    {
        var repo = new FakeRepo();
        var svc = new ProductGroupService(repo);
        var group = new ProductGroup { Id = Guid.NewGuid(), Name = "Group" };

        await svc.UpdateAsync(group);

        Assert.NotNull(repo.Updated);
    }
}
