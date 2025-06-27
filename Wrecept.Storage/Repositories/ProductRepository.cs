using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
        _db.Database.EnsureCreated();
    }

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => _db.Products.ToListAsync(ct);
}
