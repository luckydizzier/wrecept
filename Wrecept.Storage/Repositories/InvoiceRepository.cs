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

    public async Task<int> AddItemAsync(InvoiceItem item, CancellationToken ct = default)
    {
        _db.InvoiceItems.Add(item);
        await _db.SaveChangesAsync(ct);
        return item.Id;
    }

    public async Task UpdateHeaderAsync(int id, DateOnly date, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default)
    {
        var invoice = await _db.Invoices.FindAsync(new object?[] { id }, ct);
        if (invoice == null)
            return;
        invoice.Date = date;
        invoice.SupplierId = supplierId;
        invoice.PaymentMethodId = paymentMethodId;
        invoice.IsGross = isGross;
        invoice.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
    }

    public async Task SetArchivedAsync(int id, bool isArchived, CancellationToken ct = default)
    {
        var invoice = await _db.Invoices.FindAsync(new object?[] { id }, ct);
        if (invoice == null)
            return;
        invoice.IsArchived = isArchived;
        invoice.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
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
