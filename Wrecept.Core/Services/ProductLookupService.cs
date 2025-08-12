using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public class ProductLookupService : IProductLookupService
{
    private readonly AppDbContext _context;

    public ProductLookupService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            throw new ArgumentException("A keresési kifejezés nem lehet üres.", nameof(term));

        try
        {
            return await _context.Products
                .Where(p => EF.Functions.Like(p.Name, $"%{term}%"))
                .OrderBy(p => p.Name)
                .Take(20)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Nem sikerült lekérdezni a termékeket.", ex);
        }
    }
}
