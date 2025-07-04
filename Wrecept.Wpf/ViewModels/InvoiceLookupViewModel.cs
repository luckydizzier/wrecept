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

    public event Action<InvoiceLookupItem>? InvoiceSelected;

    public ObservableCollection<InvoiceLookupItem> Invoices { get; } = new();

    [ObservableProperty]
    private InvoiceLookupItem? selectedInvoice;

    partial void OnSelectedInvoiceChanged(InvoiceLookupItem? value)
    {
        if (value != null)
            InvoiceSelected?.Invoke(value);
    }

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

        if (Invoices.Count > 0)
            SelectedInvoice = Invoices[0];
    }

    public Task<int> CreateInvoiceAsync(string number)
    {
        var item = new InvoiceLookupItem
        {
            Id = 0,
            Number = number,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Supplier = string.Empty
        };

        Invoices.Insert(0, item);
        SelectedInvoice = item;
        return Task.FromResult(0);
    }

}
