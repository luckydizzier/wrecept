using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoices;

    public InvoiceService(IInvoiceRepository invoices)
    {
        _invoices = invoices;
    }

    public async Task<bool> CreateAsync(Invoice invoice, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(invoice.Number)) return false;
        if (invoice.Items.Count == 0) return false;
        if (invoice.Items.Any(i => i.Quantity <= 0)) return false;
        if (invoice.Items.Any(i => i.ProductId <= 0)) return false;
        await _invoices.AddAsync(invoice, ct);
        return true;
    }

    public Task<Invoice?> GetAsync(int id, CancellationToken ct = default)
        => _invoices.GetAsync(id, ct);
}
