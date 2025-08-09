using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Services.Dtos;

namespace Wrecept.Core.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _ctx;

    public AnalyticsService(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IReadOnlyList<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
    {
        var query = await _ctx.Invoices
            .Where(i => i.Date.Year == year)
            .GroupBy(i => i.Date.Month)
            .Select(g => new MonthlyRevenueDto(
                g.Key,
                g.Sum(i => i.TotalNet),
                g.Sum(i => i.TotalVat),
                g.Sum(i => i.TotalGross)))
            .OrderBy(r => r.Month)
            .ToListAsync();
        return query;
    }

    public async Task<IReadOnlyList<TopSupplierDto>> GetTopSuppliersAsync(int topN)
    {
        var query = await _ctx.Invoices
            .Include(i => i.Supplier)
            .GroupBy(i => i.Supplier.Name)
            .Select(g => new TopSupplierDto(g.Key, g.Sum(i => i.TotalGross)))
            .OrderByDescending(r => r.TotalGross)
            .Take(topN)
            .ToListAsync();
        return query;
    }

    public async Task<IReadOnlyList<TopProductDto>> GetTopProductsAsync(int topN)
    {
        var query = await _ctx.InvoiceItems
            .Include(i => i.Product)
            .GroupBy(i => i.Product.Name)
            .Select(g => new { Name = g.Key, Total = g.Sum(i => (double)i.TotalGross) })
            .OrderByDescending(r => r.Total)
            .Take(topN)
            .ToListAsync();

        return query
            .Select(x => new TopProductDto(x.Name, (decimal)x.Total))
            .ToList();
    }

    public async Task<TaxBreakdownDto> GetTaxBreakdownAsync()
    {
        var items = await _ctx.InvoiceItems
            .GroupBy(i => i.VatRate)
            .Select(g => new TaxBreakdownItem(
                g.Key,
                g.Sum(i => i.TotalNet),
                g.Sum(i => i.TotalVat),
                g.Sum(i => i.TotalGross)))
            .ToListAsync();

        return new TaxBreakdownDto { Items = items };
    }
}
