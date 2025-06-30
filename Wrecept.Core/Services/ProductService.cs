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

    public Task<List<Product>> GetActiveAsync(CancellationToken ct = default)
        => _products.GetActiveAsync(ct);

    public async Task<int> AddAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Name required", nameof(product));
        if (product.Net < 0 || product.Gross < 0)
            throw new ArgumentException("Price cannot be negative", nameof(product));

        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        return await _products.AddAsync(product, ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (product.Id <= 0)
            throw new ArgumentException("Invalid Id", nameof(product));
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Name required", nameof(product));
        if (product.Net < 0 || product.Gross < 0)
            throw new ArgumentException("Price cannot be negative", nameof(product));

        product.UpdatedAt = DateTime.UtcNow;
        await _products.UpdateAsync(product, ct);
    }
}
