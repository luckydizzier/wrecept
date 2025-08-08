using System.Collections.Generic;

namespace Wrecept.Core.Services.Dtos;

public record TaxBreakdownItem(decimal VatRate, decimal Net, decimal Vat, decimal Gross);

public class TaxBreakdownDto
{
    public IReadOnlyList<TaxBreakdownItem> Items { get; init; } = new List<TaxBreakdownItem>();
}
