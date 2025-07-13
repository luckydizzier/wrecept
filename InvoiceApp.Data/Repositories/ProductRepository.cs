using Microsoft.EntityFrameworkCore;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Data.Data;
using System.Text.Json;

namespace InvoiceApp.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    private void Log(string id, string op, object data)
    {
        _db.ChangeLogs.Add(new ChangeLog
        {
            Entity = nameof(Product),
            EntityId = id,
            Operation = op,
            Data = JsonSerializer.Serialize(data),
            CreatedAt = DateTime.UtcNow
        });
    }

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => _db.Products.AsNoTracking().ToListAsync(ct);

    public Task<List<Product>> GetActiveAsync(CancellationToken ct = default)
        => _db.Products.AsNoTracking().Where(p => !p.IsArchived).ToListAsync(ct);

    public async Task<int> AddAsync(Product product, CancellationToken ct = default)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);
        Log(product.Id.ToString(), "Insert", product);
        await _db.SaveChangesAsync(ct);
        return product.Id;
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync(ct);
        Log(product.Id.ToString(), "Update", product);
        await _db.SaveChangesAsync(ct);
    }
}
