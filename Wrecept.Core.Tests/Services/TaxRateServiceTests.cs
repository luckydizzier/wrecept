using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class TaxRateServiceTests
{
    private sealed class FakeRepo : ITaxRateRepository
    {
        public TaxRate? Added;
        public TaxRate? Updated;
        public Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<Guid> AddAsync(TaxRate rate, CancellationToken ct = default)
        {
            Added = rate;
            return Task.FromResult(Guid.NewGuid());
        }
        public Task UpdateAsync(TaxRate rate, CancellationToken ct = default)
        {
            Updated = rate;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task AddAsync_SetsDates()
    {
        var repo = new FakeRepo();
        var svc = new TaxRateService(repo);
        var rate = new TaxRate { Name = "ÁFA", Percentage = 27 };
        var before = DateTime.UtcNow;

        await svc.AddAsync(rate);

        Assert.NotNull(repo.Added);
        Assert.True(repo.Added!.CreatedAt >= before);
        Assert.True(repo.Added.UpdatedAt >= before);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenNameMissing()
    {
        var repo = new FakeRepo();
        var svc = new TaxRateService(repo);

        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(new TaxRate()));
    }

    [Fact]
    public async Task UpdateAsync_SetsUpdatedAt()
    {
        var repo = new FakeRepo();
        var svc = new TaxRateService(repo);
        var rate = new TaxRate { Id = Guid.NewGuid(), Name = "ÁFA", Percentage = 27 };
        var before = DateTime.UtcNow;

        await svc.UpdateAsync(rate);

        Assert.NotNull(repo.Updated);
        Assert.True(repo.Updated!.UpdatedAt >= before);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenIdInvalid()
    {
        var repo = new FakeRepo();
        var svc = new TaxRateService(repo);
        var rate = new TaxRate { Name = "ÁFA", Percentage = 27 };

        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(rate));
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenNameMissing()
    {
        var repo = new FakeRepo();
        var svc = new TaxRateService(repo);
        var rate = new TaxRate { Id = Guid.NewGuid() };

        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(rate));
    }
}
