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
        _db.Database.EnsureCreated();
    }

    public Task<List<Supplier>> GetAllAsync(CancellationToken ct = default)
        => _db.Suppliers.ToListAsync(ct);
}
