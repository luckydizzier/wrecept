using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Wrecept.Core.Repositories;

namespace Wrecept.Storage.Services;

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
            int end = last.Length - 1;
            while (end >= 0 && !char.IsDigit(last[end])) end--;
            int digitEnd = end;
            while (end >= 0 && char.IsDigit(last[end])) end--;

            if (digitEnd >= 0)
            {
                var prefix = last.Substring(0, end + 1);
                var digits = last.Substring(end + 1, digitEnd - end);
                var suffix = last.Substring(digitEnd + 1);

                if (int.TryParse(digits, out var num))
                {
                    var next = (num + 1).ToString().PadLeft(digits.Length, '0');
                    return prefix + next + suffix;
                }
            }
        }

        return "INV1";
    }
}
