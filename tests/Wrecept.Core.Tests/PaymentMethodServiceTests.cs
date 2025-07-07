using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class PaymentMethodServiceTests
{
    private class FakeRepo : IPaymentMethodRepository
    {
        public PaymentMethod? Added;
        public PaymentMethod? Updated;
        public Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<List<PaymentMethod>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<Guid> AddAsync(PaymentMethod method, CancellationToken ct = default) { Added = method; return Task.FromResult(Guid.NewGuid()); }
        public Task UpdateAsync(PaymentMethod method, CancellationToken ct = default) { Updated = method; return Task.CompletedTask; }
    }

    [Fact]
    public async Task AddAsync_SetsTimestamps()
    {
        var repo = new FakeRepo();
        var svc = new PaymentMethodService(repo);
        var method = new PaymentMethod { Name = "m", CreatedAt = DateTime.MinValue, UpdatedAt = DateTime.MinValue };

        await svc.AddAsync(method);

        Assert.Equal(method, repo.Added);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, repo.Added!.UpdatedAt);
    }

    [Fact]
    public async Task AddAsync_InvalidName_Throws()
    {
        var svc = new PaymentMethodService(new FakeRepo());
        var m = new PaymentMethod { DueInDays = 1 };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(m));
    }

    [Fact]
    public async Task AddAsync_NegativeDue_Throws()
    {
        var svc = new PaymentMethodService(new FakeRepo());
        var m = new PaymentMethod { Name = "x", DueInDays = -1 };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(m));
    }

    [Fact]
    public async Task UpdateAsync_SetsTimestamp()
    {
        var repo = new FakeRepo();
        var svc = new PaymentMethodService(repo);
        var m = new PaymentMethod { Id = Guid.NewGuid(), Name = "x", UpdatedAt = DateTime.MinValue };

        await svc.UpdateAsync(m);

        Assert.Equal(m, repo.Updated);
        Assert.NotEqual(DateTime.MinValue, repo.Updated!.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_Throws()
    {
        var svc = new PaymentMethodService(new FakeRepo());
        var m = new PaymentMethod { Id = Guid.Empty, Name = "x" };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(m));
    }
}
