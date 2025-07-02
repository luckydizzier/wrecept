using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class ProductServiceTests
{
    private sealed class FakeRepo : IProductRepository
    {
        public Product? Added;
        public Product? Updated;
        public Task<List<Product>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<Product>());
        public Task<List<Product>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<Product>());
        public Task<int> AddAsync(Product product, CancellationToken ct = default)
        {
            Added = product;
            return Task.FromResult(1);
        }
        public Task UpdateAsync(Product product, CancellationToken ct = default)
        {
            Updated = product;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task AddAsync_SetsDates()
    {
        var repo = new FakeRepo();
        var svc = new ProductService(repo);
        var prod = new Product { Name = "Prod", Net = 1, Gross = 1 };
        var before = DateTime.UtcNow;

        await svc.AddAsync(prod);

        Assert.NotNull(repo.Added);
        Assert.True(repo.Added!.CreatedAt >= before);
        Assert.True(repo.Added.UpdatedAt >= before);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenPriceNegative()
    {
        var repo = new FakeRepo();
        var svc = new ProductService(repo);
        var prod = new Product { Name = "Prod", Net = -1, Gross = -1 };

        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddAsync(prod));
    }

    [Fact]
    public async Task UpdateAsync_SetsUpdatedAt()
    {
        var repo = new FakeRepo();
        var svc = new ProductService(repo);
        var prod = new Product { Id = 1, Name = "Prod", Net = 1, Gross = 1 };
        var before = DateTime.UtcNow;

        await svc.UpdateAsync(prod);

        Assert.NotNull(repo.Updated);
        Assert.True(repo.Updated!.UpdatedAt >= before);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenIdInvalid()
    {
        var repo = new FakeRepo();
        var svc = new ProductService(repo);
        var prod = new Product { Name = "Prod", Net = 1, Gross = 1 };

        await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateAsync(prod));
    }
}
