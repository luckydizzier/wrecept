using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IInvoiceTotalsService
{
    InvoiceTotals Calculate(IEnumerable<InvoiceItem> items);
}

public record VatTotal(decimal Rate, decimal Vat);
public record InvoiceTotals(decimal Net, decimal Vat, decimal Gross, IReadOnlyList<VatTotal> ByRate);
