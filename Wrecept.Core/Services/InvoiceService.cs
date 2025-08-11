using Microsoft.Extensions.Logging;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly ILogger<InvoiceService> _logger;

    public InvoiceService(IRepository<Invoice> invoiceRepository, ILogger<InvoiceService> logger)
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    public async Task AddInvoiceAsync(Invoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);

        invoice.RecalculateTotals();

        try
        {
            await _invoiceRepository.AddAsync(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add invoice {InvoiceId}", invoice.Id);
            throw new InvalidOperationException("Adding invoice failed.", ex);
        }
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesAsync() => await _invoiceRepository.GetAllAsync();
}
