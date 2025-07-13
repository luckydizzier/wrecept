using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests;

public class TaxRateServiceTests
{
    private class FakeRepo : ITaxRateRepository
    {
        public TaxRate? Added;
        public TaxRate? Updated;
        public Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<Guid> AddAsync(TaxRate taxRate, CancellationToken ct = default) { Added = taxRate; return Task.FromResult(Guid.NewGuid()); }
        public Task UpdateAsync(TaxRate taxRate, CancellationToken ct = default) { Updated = taxRate; return Task.CompletedTask; }
    }

    [Fact]
    public async Task AddAsync_SetsTimestamps()
    {
        var repo = new FakeRepo();
        var svc = new TaxRateService(repo);
        var rate = new TaxRate { Name = "t", CreatedAt = DateTime.MinValue, UpdatedAt = DateTime.MinValue };

        await svc.AddAsync(rate);

        Assert.Equal(rate, repo.Added);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.UpdatedAt);
    }

    [Fact]
    public async Task AddAsync_InvalidName_Throws()
    {
        var svc = new TaxRateService(new FakeRepo());
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new TaxRate()));
    }

    [Fact]
    public async Task UpdateAsync_SetsTimestamp()
    {
        var repo = new FakeRepo();
        var svc = new TaxRateService(repo);
        var rate = new TaxRate { Id = Guid.NewGuid(), Name = "t", UpdatedAt = DateTime.MinValue };

        await svc.UpdateAsync(rate);

        Assert.Equal(rate, repo.Updated);
        Assert.NotEqual(DateTime.MinValue, repo.Updated!.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_Throws()
    {
        var svc = new TaxRateService(new FakeRepo());
        var rate = new TaxRate { Id = Guid.Empty, Name = "t" };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(rate));
    }
}
