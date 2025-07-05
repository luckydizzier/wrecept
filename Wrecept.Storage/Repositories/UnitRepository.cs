using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly AppDbContext _db;

    public UnitRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Unit>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<Unit>().AsNoTracking().ToListAsync(ct);

    public Task<List<Unit>> GetActiveAsync(CancellationToken ct = default)
        => _db.Set<Unit>().AsNoTracking().Where(u => !u.IsArchived).ToListAsync(ct);

    public async Task<Guid> AddAsync(Unit unit, CancellationToken ct = default)
    {
        _db.Add(unit);
        await _db.SaveChangesAsync(ct);
        return unit.Id;
    }

    public async Task UpdateAsync(Unit unit, CancellationToken ct = default)
    {
        _db.Update(unit);
        await _db.SaveChangesAsync(ct);
    }
}
