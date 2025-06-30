using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class TaxRateRepository : ITaxRateRepository
{
    private readonly AppDbContext _db;

    public TaxRateRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<TaxRate>().ToListAsync(ct);

    public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default)
        => _db.Set<TaxRate>()
            .Where(t => t.EffectiveFrom <= asOf && (!t.EffectiveTo.HasValue || t.EffectiveTo >= asOf) && !t.IsArchived)
            .OrderByDescending(t => t.EffectiveFrom)
            .ToListAsync(ct);

    public async Task<Guid> AddAsync(TaxRate taxRate, CancellationToken ct = default)
    {
        _db.Add(taxRate);
        await _db.SaveChangesAsync(ct);
        return taxRate.Id;
    }

    public async Task UpdateAsync(TaxRate taxRate, CancellationToken ct = default)
    {
        _db.Update(taxRate);
        await _db.SaveChangesAsync(ct);
    }
}
