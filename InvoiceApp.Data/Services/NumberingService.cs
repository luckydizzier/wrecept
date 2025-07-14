using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Services;
using InvoiceApp.Core.Repositories;

namespace InvoiceApp.Data.Services;

public class NumberingService : INumberingService
{
    private readonly IInvoiceRepository _invoices;

    public NumberingService(IInvoiceRepository invoices)
    {
        _invoices = invoices;
    }

    public async Task<string> GetNextInvoiceNumberAsync(int supplierId, CancellationToken ct = default)
    {
        if (supplierId <= 0)
            return "INV1";

        var last = await _invoices.GetLatestInvoiceNumberBySupplierAsync(supplierId, ct);
        if (!string.IsNullOrWhiteSpace(last))
        {
            int start = 0;
            while (start < last.Length && !char.IsDigit(last[start])) start++;
            int end = start;
            while (end < last.Length && char.IsDigit(last[end])) end++;

            if (start < end)
            {
                var prefix = last[..start];
                var digits = last[start..end];
                var suffix = last[end..];

                if (int.TryParse(digits, out var num))
                {
                    var next = (num + 1).ToString().PadLeft(digits.Length, '0');
                    return string.Concat(prefix, next, suffix);
                }
            }
        }

        return "INV1";
    }
}
