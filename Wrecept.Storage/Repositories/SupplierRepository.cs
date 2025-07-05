using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _db;

    public SupplierRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Supplier>> GetAllAsync(CancellationToken ct = default)
        => _db.Suppliers.AsNoTracking().ToListAsync(ct);

    public Task<List<Supplier>> GetActiveAsync(CancellationToken ct = default)
        => _db.Suppliers.AsNoTracking().Where(s => !s.IsArchived).ToListAsync(ct);

    public async Task<int> AddAsync(Supplier supplier, CancellationToken ct = default)
    {
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync(ct);
        return supplier.Id;
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken ct = default)
    {
        _db.Suppliers.Update(supplier);
        await _db.SaveChangesAsync(ct);
    }
}
