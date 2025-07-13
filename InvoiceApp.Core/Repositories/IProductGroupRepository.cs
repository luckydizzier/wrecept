using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Repositories;

public interface IProductGroupRepository
{
    Task<List<ProductGroup>> GetAllAsync(CancellationToken ct = default);
    Task<List<ProductGroup>> GetActiveAsync(CancellationToken ct = default);
    Task<Guid> AddAsync(ProductGroup group, CancellationToken ct = default);
    Task UpdateAsync(ProductGroup group, CancellationToken ct = default);
}
