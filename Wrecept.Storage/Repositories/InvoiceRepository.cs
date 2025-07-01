using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _db;

    public InvoiceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> AddAsync(Invoice invoice, CancellationToken ct = default)
    {
        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync(ct);
        return invoice.Id;
    }

    public Task<Invoice?> GetAsync(int id, CancellationToken ct = default)
        => _db.Invoices
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Include(i => i.Items)
                .ThenInclude(it => it.Product)
            .Include(i => i.Items)
                .ThenInclude(it => it.TaxRate)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default)
        => _db.Invoices
            .Include(i => i.Supplier)
            .OrderByDescending(i => i.Date)
            .Take(count)
            .ToListAsync(ct);
}
