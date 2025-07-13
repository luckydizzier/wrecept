using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;
using System.Text.Json;

namespace Wrecept.Storage.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _db;

    public SupplierRepository(AppDbContext db)
    {
        _db = db;
    }

    private void Log(string id, string op, object data)
    {
        _db.ChangeLogs.Add(new ChangeLog
        {
            Entity = nameof(Supplier),
            EntityId = id,
            Operation = op,
            Data = JsonSerializer.Serialize(data),
            CreatedAt = DateTime.UtcNow
        });
    }

    public Task<List<Supplier>> GetAllAsync(CancellationToken ct = default)
        => _db.Suppliers.AsNoTracking().ToListAsync(ct);

    public Task<List<Supplier>> GetActiveAsync(CancellationToken ct = default)
        => _db.Suppliers.AsNoTracking().Where(s => !s.IsArchived).ToListAsync(ct);

    public async Task<int> AddAsync(Supplier supplier, CancellationToken ct = default)
    {
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync(ct);
        Log(supplier.Id.ToString(), "Insert", supplier);
        await _db.SaveChangesAsync(ct);
        return supplier.Id;
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken ct = default)
    {
        _db.Suppliers.Update(supplier);
        await _db.SaveChangesAsync(ct);
        Log(supplier.Id.ToString(), "Update", supplier);
        await _db.SaveChangesAsync(ct);
    }
}
