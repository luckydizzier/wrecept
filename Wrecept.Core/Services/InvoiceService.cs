using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoices;
    private readonly InvoiceCalculator _calculator;

    public InvoiceService(IInvoiceRepository invoices, InvoiceCalculator calculator)
    {
        _invoices = invoices;
        _calculator = calculator;
    }

    public async Task<bool> CreateAsync(Invoice invoice, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        if (string.IsNullOrWhiteSpace(invoice.Number)) return false;
        if (invoice.SupplierId <= 0 && invoice.Supplier is null) return false;
        if (invoice.Items.Count == 0) return false;
        if (invoice.Items.Any(i => i.Quantity == 0 || i.UnitPrice < 0)) return false;
        if (invoice.Items.Any(i => i.ProductId <= 0)) return false;

        invoice.CreatedAt = DateTime.UtcNow;
        invoice.UpdatedAt = DateTime.UtcNow;
        foreach (var item in invoice.Items)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
        }

        _calculator.Calculate(invoice);

        await _invoices.AddAsync(invoice, ct);
        return true;
    }

    public Task<Invoice?> GetAsync(int id, CancellationToken ct = default)
        => _invoices.GetAsync(id, ct);

    public Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default)
        => _invoices.GetRecentAsync(count, ct);
}
