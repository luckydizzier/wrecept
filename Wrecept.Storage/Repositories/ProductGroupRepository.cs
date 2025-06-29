using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Entities;
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
}
