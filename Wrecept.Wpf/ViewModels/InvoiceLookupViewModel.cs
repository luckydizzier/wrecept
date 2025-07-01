using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class InvoiceLookupItem : ObservableObject
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Supplier { get; set; } = string.Empty;
}

public partial class InvoiceLookupViewModel : ObservableObject
{
    private readonly IInvoiceService _invoices;

    public ObservableCollection<InvoiceLookupItem> Invoices { get; } = new();

    [ObservableProperty]
    private InvoiceLookupItem? selectedInvoice;

    public InvoiceLookupViewModel(IInvoiceService invoices)
    {
        _invoices = invoices;
    }

    public async Task LoadAsync()
    {
        var items = await _invoices.GetRecentAsync(50);
        Invoices.Clear();
        foreach (var inv in items)
        {
            Invoices.Add(new InvoiceLookupItem
            {
                Id = inv.Id,
                Number = inv.Number,
                Date = inv.Date,
                Supplier = inv.Supplier?.Name ?? string.Empty
            });
        }
    }

}
