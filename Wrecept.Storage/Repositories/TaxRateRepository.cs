using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;
using System.Text.Json;

namespace Wrecept.Storage.Repositories;

public class TaxRateRepository : ITaxRateRepository
{
    private readonly AppDbContext _db;

    public TaxRateRepository(AppDbContext db)
    {
        _db = db;
    }

    private void Log(string id, string op, object data)
    {
        _db.ChangeLogs.Add(new ChangeLog
        {
            Entity = nameof(TaxRate),
            EntityId = id,
            Operation = op,
            Data = JsonSerializer.Serialize(data),
            CreatedAt = DateTime.UtcNow
        });
    }

    public Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<TaxRate>().AsNoTracking().ToListAsync(ct);

    public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default)
        => _db.Set<TaxRate>().AsNoTracking()
            .Where(t => t.EffectiveFrom <= asOf && (!t.EffectiveTo.HasValue || t.EffectiveTo >= asOf) && !t.IsArchived)
            .OrderByDescending(t => t.EffectiveFrom)
            .ToListAsync(ct);

    public async Task<Guid> AddAsync(TaxRate taxRate, CancellationToken ct = default)
    {
        _db.Add(taxRate);
        await _db.SaveChangesAsync(ct);
        Log(taxRate.Id.ToString(), "Insert", taxRate);
        await _db.SaveChangesAsync(ct);
        return taxRate.Id;
    }

    public async Task UpdateAsync(TaxRate taxRate, CancellationToken ct = default)
    {
        _db.Update(taxRate);
        await _db.SaveChangesAsync(ct);
        Log(taxRate.Id.ToString(), "Update", taxRate);
        await _db.SaveChangesAsync(ct);
    }
}
