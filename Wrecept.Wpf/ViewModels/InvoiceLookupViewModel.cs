using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Linq;
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

    [ObservableProperty]
    private object? inlinePrompt;

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

    public async Task<int> CreateInvoiceAsync(string number)
    {
        var invoice = new Invoice
        {
            Number = number,
            Date = DateOnly.FromDateTime(DateTime.Today)
        };

        var id = await _invoices.CreateHeaderAsync(invoice);

        Invoices.Insert(0, new InvoiceLookupItem
        {
            Id = id,
            Number = number,
            Date = invoice.Date,
            Supplier = string.Empty
        });

        SelectedInvoice = Invoices.FirstOrDefault(i => i.Id == id);
        return id;
    }

}
