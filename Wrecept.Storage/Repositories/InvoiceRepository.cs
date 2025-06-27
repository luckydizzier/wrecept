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
        _db.Database.EnsureCreated();
    }

    public async Task<int> AddAsync(Invoice invoice, CancellationToken ct = default)
    {
        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync(ct);
        return invoice.Id;
    }

    public Task<Invoice?> GetAsync(int id, CancellationToken ct = default)
        => _db.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id, ct);
}
