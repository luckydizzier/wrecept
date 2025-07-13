using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;

namespace InvoiceApp.Core.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _suppliers;

    public SupplierService(ISupplierRepository suppliers)
    {
        _suppliers = suppliers;
    }

    public Task<List<Supplier>> GetAllAsync(CancellationToken ct = default)
        => _suppliers.GetAllAsync(ct);

    public Task<List<Supplier>> GetActiveAsync(CancellationToken ct = default)
        => _suppliers.GetActiveAsync(ct);

    public async Task<int> AddAsync(Supplier supplier, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        if (string.IsNullOrWhiteSpace(supplier.Name))
            throw new ArgumentException("Name required", nameof(supplier));

        supplier.CreatedAt = DateTime.UtcNow;
        supplier.UpdatedAt = DateTime.UtcNow;
        return await _suppliers.AddAsync(supplier, ct);
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        if (supplier.Id <= 0)
            throw new ArgumentException("Invalid Id", nameof(supplier));
        if (string.IsNullOrWhiteSpace(supplier.Name))
            throw new ArgumentException("Name required", nameof(supplier));

        supplier.UpdatedAt = DateTime.UtcNow;
        await _suppliers.UpdateAsync(supplier, ct);
    }
}
