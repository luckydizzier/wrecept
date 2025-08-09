using System.Linq;
using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public class InvoiceTotalsService : IInvoiceTotalsService
{
    public InvoiceTotals Calculate(IEnumerable<InvoiceItem> items)
    {
        var list = items.ToList();
        var net = list.Sum(i => i.TotalNet);
        var vat = list.Sum(i => i.TotalVat);
        var gross = list.Sum(i => i.TotalGross);
        var byRate = list
            .GroupBy(i => i.VatRate)
            .Select(g => new VatTotal(g.Key, g.Sum(i => i.TotalVat)))
            .ToList();
        return new InvoiceTotals(net, vat, gross, byRate);
    }
}
