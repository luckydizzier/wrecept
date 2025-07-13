using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests;

public class ProductGroupServiceTests
{
    private class FakeRepo : IProductGroupRepository
    {
        public ProductGroup? Added;
        public ProductGroup? Updated;
        public Task<List<ProductGroup>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<List<ProductGroup>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<Guid> AddAsync(ProductGroup group, CancellationToken ct = default) { Added = group; return Task.FromResult(Guid.NewGuid()); }
        public Task UpdateAsync(ProductGroup group, CancellationToken ct = default) { Updated = group; return Task.CompletedTask; }
    }

    [Fact]
    public async Task AddAsync_SetsTimestamps()
    {
        var repo = new FakeRepo();
        var svc = new ProductGroupService(repo);
        var group = new ProductGroup { Name = "g", CreatedAt = DateTime.MinValue, UpdatedAt = DateTime.MinValue };

        await svc.AddAsync(group);

        Assert.Equal(group, repo.Added);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.UpdatedAt);
    }

    [Fact]
    public async Task AddAsync_InvalidName_Throws()
    {
        var svc = new ProductGroupService(new FakeRepo());
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new ProductGroup()));
    }

    [Fact]
    public async Task UpdateAsync_SetsTimestamp()
    {
        var repo = new FakeRepo();
        var svc = new ProductGroupService(repo);
        var group = new ProductGroup { Id = Guid.NewGuid(), Name = "g", UpdatedAt = DateTime.MinValue };

        await svc.UpdateAsync(group);

        Assert.Equal(group, repo.Updated);
        Assert.NotEqual(DateTime.MinValue, repo.Updated!.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_Throws()
    {
        var svc = new ProductGroupService(new FakeRepo());
        var group = new ProductGroup { Id = Guid.Empty, Name = "g" };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(group));
    }
}
