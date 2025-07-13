using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Services;

public class InvoiceTotals
{
    public decimal Net { get; set; }
    public decimal Tax { get; set; }
    public decimal Gross { get; set; }
}

public class InvoiceCalculationResult
{
    public decimal TotalNet { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalGross { get; set; }
    public Dictionary<Guid, InvoiceTotals> PerTaxRateBreakdown { get; } = new();
}

public class InvoiceCalculator
{
    public InvoiceCalculationResult Calculate(Invoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        var result = new InvoiceCalculationResult();

        foreach (var item in invoice.Items.Where(i => i.Quantity != 0))
        {
            var taxRate = item.TaxRate ?? item.Product?.TaxRate;
            if (taxRate is null)
                throw new InvalidOperationException("TaxRate missing");

            decimal netUnitPrice = invoice.IsGross
                ? item.UnitPrice / (1 + taxRate.Percentage / 100m)
                : item.UnitPrice;

            decimal netAmount = item.Quantity * netUnitPrice;
            decimal taxAmount = netAmount * (taxRate.Percentage / 100m);
            decimal grossAmount = netAmount + taxAmount;

            result.TotalNet += netAmount;
            result.TotalTax += taxAmount;
            result.TotalGross += grossAmount;

            if (!result.PerTaxRateBreakdown.TryGetValue(taxRate.Id, out var totals))
            {
                totals = new InvoiceTotals();
                result.PerTaxRateBreakdown[taxRate.Id] = totals;
            }
            totals.Net += netAmount;
            totals.Tax += taxAmount;
            totals.Gross += grossAmount;
        }

        return result;
    }
}
