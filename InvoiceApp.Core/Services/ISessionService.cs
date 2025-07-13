namespace InvoiceApp.Core.Services;

public interface ISessionService
{
    Task<int?> LoadLastInvoiceIdAsync(CancellationToken ct = default);
    Task SaveLastInvoiceIdAsync(int? invoiceId, CancellationToken ct = default);
}
