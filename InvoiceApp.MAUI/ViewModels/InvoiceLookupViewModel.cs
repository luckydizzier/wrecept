using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Linq;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.MAUI.ViewModels;

public partial class InvoiceLookupItem : ObservableObject
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public int SupplierId { get; set; }
}

public partial class InvoiceLookupViewModel : ObservableObject
{
    private readonly IInvoiceService _invoices;
    private readonly INumberingService _numbering;

    public event Action<InvoiceLookupItem>? InvoiceSelected;
    public event Action<string>? InvoiceCreated;

    public ObservableCollection<InvoiceLookupItem> Invoices { get; } = new();

    [ObservableProperty]
    private InvoiceLookupItem? selectedInvoice;

    partial void OnSelectedInvoiceChanged(InvoiceLookupItem? value)
    {
        if (value != null)
            InvoiceSelected?.Invoke(value);
    }

    [RelayCommand]
    private async Task CreateNewInvoiceAsync()
    {
        var supplierId = SelectedInvoice?.SupplierId ?? 0;
        var number = await _numbering.GetNextInvoiceNumberAsync(supplierId);
        await CreateInvoiceAsync(number);
    }

    public InvoiceLookupViewModel(IInvoiceService invoices, INumberingService numbering)
    {
        _invoices = invoices;
        _numbering = numbering;
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
                    Supplier = inv.Supplier?.Name ?? string.Empty,
                    SupplierId = inv.SupplierId
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
        InvoiceCreated?.Invoke(number);
        return Task.FromResult(0);
    }


}
