using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests;

public class ProductServiceTests
{
    private class FakeRepo : IProductRepository
    {
        public Product? Added;
        public Product? Updated;
        public Task<List<Product>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<Product>());
        public Task<List<Product>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<Product>());
        public Task<int> AddAsync(Product product, CancellationToken ct = default) { Added = product; return Task.FromResult(1); }
        public Task UpdateAsync(Product product, CancellationToken ct = default) { Updated = product; return Task.CompletedTask; }
    }

    [Fact]
    public async Task AddAsync_SetsTimestamps()
    {
        var repo = new FakeRepo();
        var svc = new ProductService(repo);
        var product = new Product { Name = "p", Net = 1m, Gross = 1m, CreatedAt = DateTime.MinValue, UpdatedAt = DateTime.MinValue };

        await svc.AddAsync(product);

        Assert.Equal(product, repo.Added);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.UpdatedAt);
    }

    [Fact]
    public async Task AddAsync_InvalidName_Throws()
    {
        var svc = new ProductService(new FakeRepo());
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new Product { Net = 1, Gross = 1 }));
    }

    [Fact]
    public async Task AddAsync_NegativePrice_Throws()
    {
        var svc = new ProductService(new FakeRepo());
        var p = new Product { Name = "x", Net = -1m, Gross = 1m };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(p));
    }

    [Fact]
    public async Task UpdateAsync_SetsTimestamp()
    {
        var repo = new FakeRepo();
        var svc = new ProductService(repo);
        var p = new Product { Id = 1, Name = "p", Net = 1m, Gross = 1m, UpdatedAt = DateTime.MinValue };

        await svc.UpdateAsync(p);

        Assert.Equal(p, repo.Updated);
        Assert.NotEqual(DateTime.MinValue, repo.Updated!.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_Throws()
    {
        var svc = new ProductService(new FakeRepo());
        var p = new Product { Id = 0, Name = "p", Net = 1m, Gross = 1m };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(p));
    }
}
