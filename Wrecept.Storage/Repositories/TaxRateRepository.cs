using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Entities;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class TaxRateRepository : ITaxRateRepository
{
    private readonly AppDbContext _db;

    public TaxRateRepository(AppDbContext db)
    {
        _db = db;
        _db.Database.EnsureCreated();
    }

    public Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<TaxRate>().ToListAsync(ct);
}
