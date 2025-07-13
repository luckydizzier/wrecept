using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Services;

public interface IInvoiceExportService
{
    Task SavePdfAsync(Invoice invoice, string filePath, CancellationToken ct = default);
    Task PrintAsync(Invoice invoice, CancellationToken ct = default);
}
