using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;

namespace Wrecept.Core.Services;

public class TaxService : ITaxService
{
    private readonly AppDbContext _context;

    public TaxService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<decimal>> GetRatesAsync()
    {
        return await _context.Products
            .Select(p => p.VatRate)
            .Distinct()
            .OrderBy(r => r)
            .ToListAsync();
    }
}
