using System.Linq;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class InvoiceTotalsServiceTests
{
    [Fact]
    public void Calculates_totals_and_vat_breakdown()
    {
        var service = new InvoiceTotalsService();
        var items = new[]
        {
            new InvoiceItem { Quantity = 2, UnitPrice = 100m, VatRate = 0.27m },
            new InvoiceItem { Quantity = 1, UnitPrice = 50m, VatRate = 0.05m }
        };

        var totals = service.Calculate(items);

        Assert.Equal(250m, totals.Net);
        var expectedVat = 2 * 100m * 0.27m + 50m * 0.05m;
        Assert.Equal(expectedVat, totals.Vat);
        Assert.Equal(totals.Net + expectedVat, totals.Gross);
        Assert.Equal(2, totals.ByRate.Count);
        var vat27 = totals.ByRate.Single(v => v.Rate == 0.27m).Vat;
        Assert.Equal(2 * 100m * 0.27m, vat27);
    }
}
