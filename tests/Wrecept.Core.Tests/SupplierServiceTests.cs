using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests;

public class SupplierServiceTests
{
    private class FakeRepo : ISupplierRepository
    {
        public Supplier? Added;
        public Supplier? Updated;
        public Task<List<Supplier>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<List<Supplier>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<int> AddAsync(Supplier supplier, CancellationToken ct = default) { Added = supplier; return Task.FromResult(1); }
        public Task UpdateAsync(Supplier supplier, CancellationToken ct = default) { Updated = supplier; return Task.CompletedTask; }
    }

    [Fact]
    public async Task AddAsync_SetsTimestamps()
    {
        var repo = new FakeRepo();
        var svc = new SupplierService(repo);
        var sup = new Supplier { Name = "s", CreatedAt = DateTime.MinValue, UpdatedAt = DateTime.MinValue };

        await svc.AddAsync(sup);

        Assert.Equal(sup, repo.Added);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.UpdatedAt);
    }

    [Fact]
    public async Task AddAsync_InvalidName_Throws()
    {
        var svc = new SupplierService(new FakeRepo());
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new Supplier()));
    }

    [Fact]
    public async Task UpdateAsync_SetsTimestamp()
    {
        var repo = new FakeRepo();
        var svc = new SupplierService(repo);
        var sup = new Supplier { Id = 1, Name = "s", UpdatedAt = DateTime.MinValue };

        await svc.UpdateAsync(sup);

        Assert.Equal(sup, repo.Updated);
        Assert.NotEqual(DateTime.MinValue, repo.Updated!.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_Throws()
    {
        var svc = new SupplierService(new FakeRepo());
        var sup = new Supplier { Id = 0, Name = "s" };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(sup));
    }
}
