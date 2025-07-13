using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Repositories;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync(CancellationToken ct = default);
    Task<List<Supplier>> GetActiveAsync(CancellationToken ct = default);
    Task<int> AddAsync(Supplier supplier, CancellationToken ct = default);
    Task UpdateAsync(Supplier supplier, CancellationToken ct = default);
}
