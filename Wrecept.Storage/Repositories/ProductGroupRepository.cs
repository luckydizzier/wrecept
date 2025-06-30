using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class ProductGroupRepository : IProductGroupRepository
{
    private readonly AppDbContext _db;

    public ProductGroupRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<ProductGroup>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<ProductGroup>().ToListAsync(ct);

    public Task<List<ProductGroup>> GetActiveAsync(CancellationToken ct = default)
        => _db.Set<ProductGroup>().Where(g => !g.IsArchived).ToListAsync(ct);

    public async Task<Guid> AddAsync(ProductGroup group, CancellationToken ct = default)
    {
        _db.Add(group);
        await _db.SaveChangesAsync(ct);
        return group.Id;
    }

    public async Task UpdateAsync(ProductGroup group, CancellationToken ct = default)
    {
        _db.Update(group);
        await _db.SaveChangesAsync(ct);
    }
}
