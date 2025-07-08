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
        if (invoice.Items.Any(i => i.TaxRate is null && (i.Product?.TaxRate is null)))
            return false;

        invoice.CreatedAt = DateTime.UtcNow;
        invoice.UpdatedAt = DateTime.UtcNow;
        if (invoice.DueDate == default && invoice.PaymentMethod != null)
            invoice.DueDate = invoice.Date.AddDays(invoice.PaymentMethod.DueInDays);
        foreach (var item in invoice.Items)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
        }

        _calculator.Calculate(invoice);

        await _invoices.AddAsync(invoice, ct);
        return true;
    }

    public async Task<int> CreateHeaderAsync(Invoice invoice, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        if (string.IsNullOrWhiteSpace(invoice.Number))
            throw new ArgumentException("Number required", nameof(invoice));

        invoice.CreatedAt = DateTime.UtcNow;
        invoice.UpdatedAt = DateTime.UtcNow;
        if (invoice.DueDate == default && invoice.PaymentMethod != null)
            invoice.DueDate = invoice.Date.AddDays(invoice.PaymentMethod.DueInDays);
        return await _invoices.AddAsync(invoice, ct);
    }

    public async Task<int> AddItemAsync(InvoiceItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.InvoiceId <= 0 || item.ProductId <= 0)
            throw new ArgumentException("Invalid item");

        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        return await _invoices.AddItemAsync(item, ct);
    }

    public Task RemoveItemAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid id", nameof(id));
        return _invoices.RemoveItemAsync(id, ct);
    }

    public Task UpdateInvoiceHeaderAsync(int id, string number, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid id", nameof(id));
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Number required", nameof(number));
        if (supplierId <= 0)
            throw new ArgumentException("Supplier required", nameof(supplierId));
        if (paymentMethodId == Guid.Empty)
            throw new ArgumentException("Payment method required", nameof(paymentMethodId));
        if (date == default)
            throw new ArgumentException("Date required", nameof(date));

        return _invoices.UpdateHeaderAsync(id, number, date, dueDate, supplierId, paymentMethodId, isGross, ct);
    }

    public Task ArchiveAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid id", nameof(id));
        return _invoices.SetArchivedAsync(id, true, ct);
    }

    public Task<Invoice?> GetAsync(int id, CancellationToken ct = default)
        => _invoices.GetAsync(id, ct);

    public Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default)
        => _invoices.GetRecentAsync(count, ct);

    public Task<LastUsageData?> GetLastUsageDataAsync(int supplierId, int productId, CancellationToken ct = default)
    {
        if (supplierId <= 0)
            throw new ArgumentException("supplierId", nameof(supplierId));
        if (productId <= 0)
            throw new ArgumentException("productId", nameof(productId));

        return _invoices.GetLastUsageDataAsync(supplierId, productId, ct);
    }

    public Task<Dictionary<int, LastUsageData>> GetLastUsageDataBatchAsync(int supplierId, IEnumerable<int> productIds, CancellationToken ct = default)
    {
        if (supplierId <= 0)
            throw new ArgumentException("supplierId", nameof(supplierId));
        if (productIds == null)
            throw new ArgumentNullException(nameof(productIds));

        return _invoices.GetLastUsageDataBatchAsync(supplierId, productIds, ct);
    }

    public InvoiceCalculationResult RecalculateTotals(Invoice invoice)
        => _calculator.Calculate(invoice);
}
