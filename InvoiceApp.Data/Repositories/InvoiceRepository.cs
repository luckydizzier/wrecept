using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Data.Data;

namespace InvoiceApp.Data.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _db;

    public InvoiceRepository(AppDbContext db)
    {
        _db = db;
    }

    private void Log(string entity, string id, string op, object data)
    {
        _db.ChangeLogs.Add(new ChangeLog
        {
            Entity = entity,
            EntityId = id,
            Operation = op,
            Data = JsonSerializer.Serialize(data),
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<int> AddAsync(Invoice invoice, CancellationToken ct = default)
    {
        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync(ct);
        Log(nameof(Invoice), invoice.Id.ToString(), "Insert", invoice);
        await _db.SaveChangesAsync(ct);
        return invoice.Id;
    }

    async Task<int> IInvoiceRepository.AddItemAsync(InvoiceItem item, CancellationToken ct)
        => await AddItemInternalAsync(item, ct);

    private async Task<int> AddItemInternalAsync(InvoiceItem item, CancellationToken ct = default)
    {
        _db.InvoiceItems.Add(item);
        await _db.SaveChangesAsync(ct);
        Log(nameof(InvoiceItem), item.Id.ToString(), "Insert", item);
        await _db.SaveChangesAsync(ct);
        return item.Id;
    }

    public async Task RemoveItemAsync(int id, CancellationToken ct = default)
    {
        var item = await _db.InvoiceItems.FindAsync(new object?[] { id }, ct);
        if (item == null)
            return;
        Log(nameof(InvoiceItem), id.ToString(), "Delete", item);
        _db.InvoiceItems.Remove(item);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateHeaderAsync(int id, string number, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default)
    {
        var invoice = await _db.Invoices.FindAsync(new object?[] { id }, ct);
        if (invoice == null)
            return;
        invoice.Number = number;
        invoice.Date = date;
        invoice.DueDate = dueDate;
        invoice.SupplierId = supplierId;
        invoice.PaymentMethodId = paymentMethodId;
        invoice.IsGross = isGross;
        invoice.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        Log(nameof(Invoice), id.ToString(), "Update", invoice);
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
        Log(nameof(Invoice), id.ToString(), "Update", invoice);
        await _db.SaveChangesAsync(ct);
    }

    public Task<Invoice?> GetAsync(int id, CancellationToken ct = default)
        => _db.Invoices.AsNoTracking()
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Include(i => i.Items)
                .ThenInclude(it => it.Product)
            .Include(i => i.Items)
                .ThenInclude(it => it.TaxRate)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default)
        => _db.Invoices.AsNoTracking()
            .Include(i => i.Supplier)
            .OrderByDescending(i => i.Date)
            .Take(count)
            .ToListAsync(ct);

    public Task<string?> GetLatestInvoiceNumberBySupplierAsync(int supplierId, CancellationToken ct = default)
        => _db.Invoices.AsNoTracking()
            .Where(i => i.SupplierId == supplierId)
            .OrderByDescending(i => i.Date)
            .ThenByDescending(i => i.Id)
            .Select(i => i.Number)
            .FirstOrDefaultAsync(ct);

    public async Task<LastUsageData?> GetLastUsageDataAsync(int supplierId, int productId, CancellationToken ct = default)
    {
        return await _db.InvoiceItems.AsNoTracking()
            .Include(i => i.Invoice)
            .Where(i => i.Invoice!.SupplierId == supplierId && i.ProductId == productId)
            .OrderByDescending(i => i.Invoice!.Date)
            .Select(i => new LastUsageData
            {
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TaxRateId = i.TaxRateId
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Dictionary<int, LastUsageData>> GetLastUsageDataBatchAsync(int supplierId, IEnumerable<int> productIds, CancellationToken ct = default)
    {
        var list = await _db.InvoiceItems.AsNoTracking()
            .Include(i => i.Invoice)
            .Where(i => i.Invoice!.SupplierId == supplierId && productIds.Contains(i.ProductId))
            .GroupBy(i => i.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Data = g.OrderByDescending(x => x.Invoice!.Date).Select(x => new LastUsageData
                {
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TaxRateId = x.TaxRateId
                }).First()
            })
            .ToListAsync(ct);

        return list.ToDictionary(x => x.ProductId, x => x.Data);
    }
}
