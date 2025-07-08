using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Services;
using Wrecept.Core.Models;
using Wrecept.Core.Utilities;

namespace Wrecept.Wpf.ViewModels;

public partial class VatSummaryRowViewModel : ObservableObject
{
    [ObservableProperty]
    private string rate = string.Empty;

    [ObservableProperty]
    private decimal net;

    [ObservableProperty]
    private decimal vat;

    [ObservableProperty]
    private decimal gross;
}

public partial class TotalsViewModel : ObservableObject
{
    public ObservableCollection<VatSummaryRowViewModel> VatSummaries { get; } = new();

    [ObservableProperty]
    private decimal netTotal;

    [ObservableProperty]
    private decimal vatTotal;

    [ObservableProperty]
    private decimal grossTotal;

    [ObservableProperty]
    private string amountInWords = string.Empty;

    public void Recalculate(IEnumerable<InvoiceItemRowViewModel> items, IEnumerable<TaxRate> taxRates, bool isGross)
    {
        decimal net = 0;
        decimal vat = 0;
        decimal gross = 0;
        VatSummaries.Clear();
        var byTax = new Dictionary<Guid, InvoiceTotals>();

        foreach (var row in items)
        {
            var tax = taxRates.FirstOrDefault(t => t.Id == row.TaxRateId);
            if (tax is null) continue;

            decimal netUnit = isGross
                ? row.UnitPrice / (1 + tax.Percentage / 100m)
                : row.UnitPrice;

            decimal netAmount = row.Quantity * netUnit;
            decimal vatAmount = netAmount * (tax.Percentage / 100m);
            decimal grossAmount = netAmount + vatAmount;

            net += netAmount;
            vat += vatAmount;
            gross += grossAmount;

            if (!byTax.TryGetValue(tax.Id, out var totals))
            {
                totals = new InvoiceTotals();
                byTax[tax.Id] = totals;
            }
            totals.Net += netAmount;
            totals.Tax += vatAmount;
            totals.Gross += grossAmount;
        }

        NetTotal = Math.Round(net, 2, MidpointRounding.AwayFromZero);
        VatTotal = Math.Round(vat, 2, MidpointRounding.AwayFromZero);
        GrossTotal = Math.Round(gross, 2, MidpointRounding.AwayFromZero);
        foreach (var kv in byTax)
        {
            var name = taxRates.FirstOrDefault(t => t.Id == kv.Key)?.Name ?? string.Empty;
            VatSummaries.Add(new VatSummaryRowViewModel
            {
                Rate = name,
                Net = Math.Round(kv.Value.Net, 2, MidpointRounding.AwayFromZero),
                Vat = Math.Round(kv.Value.Tax, 2, MidpointRounding.AwayFromZero),
                Gross = Math.Round(kv.Value.Gross, 2, MidpointRounding.AwayFromZero)
            });
        }
        AmountInWords = NumberToWordsConverter.Convert((long)GrossTotal) + " Ft";
    }
}
