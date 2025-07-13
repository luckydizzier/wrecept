using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(CancellationToken ct = default);
    Task<List<Product>> GetActiveAsync(CancellationToken ct = default);
    Task<int> AddAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
}
