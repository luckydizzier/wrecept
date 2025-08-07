using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IRepository<Invoice> _invoiceRepository;

    public InvoiceService(IRepository<Invoice> invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task AddInvoiceAsync(Invoice invoice) => await _invoiceRepository.AddAsync(invoice);

    public async Task<IEnumerable<Invoice>> GetInvoicesAsync() => await _invoiceRepository.GetAllAsync();
}
