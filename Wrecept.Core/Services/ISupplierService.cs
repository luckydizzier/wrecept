using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface ISupplierService
{
    Task<List<Supplier>> GetAllAsync(CancellationToken ct = default);
    Task<List<Supplier>> GetActiveAsync(CancellationToken ct = default);
}
