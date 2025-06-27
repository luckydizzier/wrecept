using Wrecept.Core.Models;

namespace Wrecept.Core.Repositories;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync(CancellationToken ct = default);
}
