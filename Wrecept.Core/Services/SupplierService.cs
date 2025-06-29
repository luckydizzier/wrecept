using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _suppliers;

    public SupplierService(ISupplierRepository suppliers)
    {
        _suppliers = suppliers;
    }

    public Task<List<Supplier>> GetAllAsync(CancellationToken ct = default)
        => _suppliers.GetAllAsync(ct);
}
