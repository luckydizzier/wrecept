using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

public class PaymentMethodServiceTests
{
    private sealed class FakeRepo : IPaymentMethodRepository
    {
        public PaymentMethod? Added;
        public PaymentMethod? Updated;
        public Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<List<PaymentMethod>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<Guid> AddAsync(PaymentMethod method, CancellationToken ct = default)
        {
            Added = method;
            return Task.FromResult(Guid.NewGuid());
        }
        public Task UpdateAsync(PaymentMethod method, CancellationToken ct = default)
        {
            Updated = method;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task AddAsync_SetsDates()
    {
        var repo = new FakeRepo();
        var svc = new PaymentMethodService(repo);
        var method = new PaymentMethod { Name = "Cash" };
        var before = DateTime.UtcNow;

        await svc.AddAsync(method);

        Assert.NotNull(repo.Added);
        Assert.True(repo.Added!.CreatedAt >= before);
        Assert.True(repo.Added.UpdatedAt >= before);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenNameMissing()
    {
        var repo = new FakeRepo();
        var svc = new PaymentMethodService(repo);
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new PaymentMethod()));
    }

    [Fact]
    public async Task UpdateAsync_SetsUpdatedAt()
    {
        var repo = new FakeRepo();
        var svc = new PaymentMethodService(repo);
        var method = new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash" };
        var before = DateTime.UtcNow;

        await svc.UpdateAsync(method);

        Assert.NotNull(repo.Updated);
        Assert.True(repo.Updated!.UpdatedAt >= before);
    }

    [Fact]
    public async Task UpdateAsync_AllowsArchiving()
    {
        var repo = new FakeRepo();
        var svc = new PaymentMethodService(repo);
        var method = new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash", IsArchived = true };

        await svc.UpdateAsync(method);

        Assert.True(repo.Updated?.IsArchived);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenIdInvalid()
    {
        var repo = new FakeRepo();
        var svc = new PaymentMethodService(repo);
        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(new PaymentMethod { Name = "Cash" }));
    }
}
