using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Desktop.ViewModels;

public partial class InvoiceEditorViewModel : ObservableObject
{
    private readonly IInvoiceService _service;

    [ObservableProperty]
    private string number = string.Empty;

    [ObservableProperty]
    private DateOnly date = DateOnly.FromDateTime(DateTime.Today);

    [ObservableProperty]
    private string supplierName = string.Empty;

    public ObservableCollection<ItemRow> Items { get; } = new();

    public IRelayCommand SaveCommand { get; }

    public InvoiceEditorViewModel(IInvoiceService service)
    {
        _service = service;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
        Items.Add(new ItemRow());
    }

    private async Task SaveAsync()
    {
        var invoice = new Invoice
        {
            Number = Number,
            Date = Date,
            Supplier = new Supplier { Name = SupplierName },
            Items = Items.Select(i => new InvoiceItem
            {
                ProductId = i.ProductId,
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        var success = await _service.CreateAsync(invoice);
        if (!success)
        {
            System.Diagnostics.Debug.WriteLine("Hibás számla adatok");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Számla mentve");
        }
    }
}

public partial class ItemRow : ObservableObject
{
    [ObservableProperty] private int productId;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private decimal quantity = 1;
    [ObservableProperty] private decimal unitPrice;
}
