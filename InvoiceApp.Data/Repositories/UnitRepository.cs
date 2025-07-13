using Microsoft.EntityFrameworkCore;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Data.Data;
using System.Text.Json;

namespace InvoiceApp.Data.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly AppDbContext _db;

    public UnitRepository(AppDbContext db)
    {
        _db = db;
    }

    private void Log(string id, string op, object data)
    {
        _db.ChangeLogs.Add(new ChangeLog
        {
            Entity = nameof(Unit),
            EntityId = id,
            Operation = op,
            Data = JsonSerializer.Serialize(data),
            CreatedAt = DateTime.UtcNow
        });
    }

    public Task<List<Unit>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<Unit>().AsNoTracking().ToListAsync(ct);

    public Task<List<Unit>> GetActiveAsync(CancellationToken ct = default)
        => _db.Set<Unit>().AsNoTracking().Where(u => !u.IsArchived).ToListAsync(ct);

    public async Task<Guid> AddAsync(Unit unit, CancellationToken ct = default)
    {
        _db.Add(unit);
        await _db.SaveChangesAsync(ct);
        Log(unit.Id.ToString(), "Insert", unit);
        await _db.SaveChangesAsync(ct);
        return unit.Id;
    }

    public async Task UpdateAsync(Unit unit, CancellationToken ct = default)
    {
        _db.Update(unit);
        await _db.SaveChangesAsync(ct);
        Log(unit.Id.ToString(), "Update", unit);
        await _db.SaveChangesAsync(ct);
    }
}
