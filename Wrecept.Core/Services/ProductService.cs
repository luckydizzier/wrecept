using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;

    public ProductService(IProductRepository products)
    {
        _products = products;
    }

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => _products.GetAllAsync(ct);

    public async Task<int> AddAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Name required", nameof(product));
        return await _products.AddAsync(product, ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (product.Id <= 0)
            throw new ArgumentException("Invalid Id", nameof(product));
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Name required", nameof(product));
        await _products.UpdateAsync(product, ct);
    }
}
