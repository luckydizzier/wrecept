using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;
using Wrecept.Wpf.Resources;

namespace Wrecept.Wpf.ViewModels;

public partial class InvoiceItemRowViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;

    public InvoiceItemRowViewModel(InvoiceEditorViewModel parent)
    {
        _parent = parent;
    }

    [ObservableProperty]
    private string product = string.Empty;

    partial void OnProductChanged(string value)
        => _ = _parent.CheckProductAsync(this, value);

    [ObservableProperty]
    private decimal quantity;

    [ObservableProperty]
    private decimal unitPrice;

    [ObservableProperty]
    private Guid taxRateId;

    [ObservableProperty]
    private string taxRateName = string.Empty;

    partial void OnTaxRateIdChanged(Guid value)
        => TaxRateName = _parent.TaxRates.FirstOrDefault(t => t.Id == value)?.Name ?? string.Empty;

    [ObservableProperty]
    private Guid unitId;

    [ObservableProperty]
    private string unitName = string.Empty;

    partial void OnUnitIdChanged(Guid value)
        => UnitName = _parent.Units.FirstOrDefault(u => u.Id == value)?.Name ?? string.Empty;

    [ObservableProperty]
    private string productGroup = string.Empty;

    [ObservableProperty]
    private bool isEditable = true;
}

public partial class InvoiceEditorViewModel : ObservableObject
{
    public ObservableCollection<InvoiceItemRowViewModel> Items { get; }

    public InvoiceLookupViewModel Lookup { get; }

    public ObservableCollection<PaymentMethod> PaymentMethods { get; } = new();
    public ObservableCollection<TaxRate> TaxRates { get; } = new();
    public ObservableCollection<Supplier> Suppliers { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<Unit> Units { get; } = new();

    [ObservableProperty]
    private string supplier = string.Empty;
partial void OnSupplierChanged(string value) => UpdateSupplierId(value);

    [ObservableProperty]
    private int supplierId;

    [ObservableProperty]
    private DateTime? invoiceDate;

    [ObservableProperty]
    private string number = string.Empty;

    [ObservableProperty]
    private Guid paymentMethodId;

    [ObservableProperty]
    private bool isGross;

    [ObservableProperty]
    private bool isArchived;

    public bool IsEditable => !IsArchived;

    partial void OnIsArchivedChanged(bool value) => OnPropertyChanged(nameof(IsEditable));

    private readonly IPaymentMethodService _paymentMethods;
    private readonly ITaxRateService _taxRates;
    private readonly ISupplierService _suppliers;
    private readonly IProductService _productsService;
    private readonly IUnitService _unitsService;
    private readonly IInvoiceService _invoiceService;

    [ObservableProperty]
    private object? inlineCreator;

    public InvoiceEditorViewModel(
        IPaymentMethodService paymentMethods,
        ITaxRateService taxRates,
        ISupplierService suppliers,
        IProductService products,
        IUnitService units,
        IInvoiceService invoiceService,
        InvoiceLookupViewModel lookup)
    {
        _paymentMethods = paymentMethods;
        _taxRates = taxRates;
        _suppliers = suppliers;
        _productsService = products;
        _unitsService = units;
        _invoiceService = invoiceService;
        Lookup = lookup;
        Lookup.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(Lookup.SelectedInvoice))
                LookupLoadSelected();
        };
        Items = new ObservableCollection<InvoiceItemRowViewModel>(
            Enumerable.Range(1, 3).Select(i => new InvoiceItemRowViewModel(this) { IsEditable = i == 1 }));
    }

    public async Task LoadAsync(IProgress<ProgressReport>? progress = null)
    {
        progress?.Report(new ProgressReport { SubtaskPercent = 0, Message = Resources.Strings.Load_PaymentMethods });
        var methods = await _paymentMethods.GetActiveAsync();
        PaymentMethods.Clear();
        foreach (var m in methods)
            PaymentMethods.Add(m);

        progress?.Report(new ProgressReport { SubtaskPercent = 20, Message = Resources.Strings.Load_Suppliers });
        var supplierItems = await _suppliers.GetActiveAsync();
        Suppliers.Clear();
        foreach (var s in supplierItems)
            Suppliers.Add(s);

        progress?.Report(new ProgressReport { SubtaskPercent = 40, Message = Resources.Strings.Load_TaxRates });
        var taxRates = await _taxRates.GetActiveAsync(DateTime.UtcNow);
        TaxRates.Clear();
        foreach (var t in taxRates)
            TaxRates.Add(t);

        progress?.Report(new ProgressReport { SubtaskPercent = 60, Message = Resources.Strings.Load_Products });
        var productItems = await _productsService.GetActiveAsync();
        Products.Clear();
        foreach (var p in productItems)
            Products.Add(p);

        progress?.Report(new ProgressReport { SubtaskPercent = 80, Message = Resources.Strings.Load_Units });
        var unitItems = await _unitsService.GetActiveAsync();
        Units.Clear();
        foreach (var u in unitItems)
            Units.Add(u);
        progress?.Report(new ProgressReport { SubtaskPercent = 100, Message = Resources.Strings.Load_Complete });
    }

    public Task CheckProductAsync(InvoiceItemRowViewModel row, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Task.CompletedTask;

        var exists = Products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (exists is null)
        {
            InlineCreator = new ProductCreatorViewModel(this, row, _productsService)
            {
                Name = name
            };
        }
        else
        {
            row.Product = exists.Name;
            row.UnitId = exists.UnitId;
            row.UnitName = _parent.Units.FirstOrDefault(u => u.Id == exists.UnitId)?.Name ?? string.Empty;
            row.ProductGroup = exists.ProductGroup?.Name ?? string.Empty;
            row.TaxRateId = exists.TaxRateId;
            row.TaxRateName = _parent.TaxRates.FirstOrDefault(t => t.Id == exists.TaxRateId)?.Name ?? string.Empty;
        }

        return Task.CompletedTask;
    }
private void UpdateSupplierId(string name)
{
    var match = Suppliers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    if (match != null)
        SupplierId = match.Id;
}
    [RelayCommand]
    private void ShowSupplierCreator(string? name)
    {
        InlineCreator = new SupplierCreatorViewModel(this, _suppliers)
        {
            Name = name ?? string.Empty
        };
    }
    [RelayCommand]
    private void ShowProductCreator(InvoiceItemRowViewModel row)
    {
        InlineCreator = new ProductCreatorViewModel(this, row, _productsService)
        {
            Name = row.Product
        };
    }

    [RelayCommand]
    private void ShowTaxRateCreator(string name)
    {
        InlineCreator = new TaxRateCreatorViewModel(this, _taxRates)
        {
            Name = name
        };
    }

    [RelayCommand]
    private void ShowUnitCreator()
    {
        InlineCreator = new UnitCreatorViewModel(this, _unitsService);
    }

    [RelayCommand]
    private void ShowPaymentMethodCreator()
    {
        InlineCreator = new PaymentMethodCreatorViewModel(this, _paymentMethods);
    }

    public async Task LoadInvoice(int id)
    {
        var invoice = await _invoiceService.GetAsync(id);
        if (invoice == null)
            return;

        SupplierId = invoice.SupplierId;
        Supplier = invoice.Supplier?.Name ?? string.Empty;
        InvoiceDate = invoice.Date.ToDateTime(TimeOnly.MinValue);
        Number = invoice.Number;
        PaymentMethodId = invoice.PaymentMethodId;
        IsGross = invoice.IsGross;
        IsArchived = invoice.IsArchived;

        Items.Clear();
        Items.Add(new InvoiceItemRowViewModel(this) { IsEditable = true });
        foreach (var item in invoice.Items)
        {
            var row = new InvoiceItemRowViewModel(this)
            {
                Product = item.Product?.Name ?? string.Empty,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TaxRateId = item.TaxRateId,
                UnitId = item.Product?.UnitId ?? Guid.Empty,
                UnitName = item.Product?.Unit?.Name ?? string.Empty,
                TaxRateName = item.TaxRate?.Name ?? string.Empty,
                ProductGroup = item.Product?.ProductGroup?.Name ?? string.Empty,
                IsEditable = false
            };
            Items.Add(row);
        }
    }

    public void EditLineFromSelection(InvoiceItemRowViewModel selected)
    {
        if (Items.IndexOf(selected) <= 0) return;
        var edit = Items[0];
        edit.Product = selected.Product;
        edit.Quantity = selected.Quantity;
        edit.UnitPrice = selected.UnitPrice;
        edit.TaxRateId = selected.TaxRateId;
        edit.UnitId = selected.UnitId;
        edit.UnitName = selected.UnitName;
        edit.TaxRateName = selected.TaxRateName;
        edit.ProductGroup = selected.ProductGroup;
    }

    [RelayCommand]
    private void AddLineItem()
    {
        var edit = Items[0];
        if (string.IsNullOrWhiteSpace(edit.Product)) return;

        var row = new InvoiceItemRowViewModel(this)
        {
            Product = edit.Product,
            Quantity = edit.Quantity,
            UnitPrice = edit.UnitPrice,
            TaxRateId = edit.TaxRateId,
            UnitId = edit.UnitId,
            UnitName = edit.UnitName,
            TaxRateName = edit.TaxRateName,
            ProductGroup = edit.ProductGroup,
            IsEditable = false
        };

        Items.Add(row);
        edit.Product = string.Empty;
        edit.Quantity = 0;
        edit.UnitPrice = 0;
        edit.TaxRateId = Guid.Empty;
        edit.UnitId = Guid.Empty;
        edit.UnitName = string.Empty;
        edit.TaxRateName = string.Empty;
        edit.ProductGroup = string.Empty;
    }

    private async void LookupLoadSelected()
    {
        if (Lookup.SelectedInvoice != null)
            await LoadInvoice(Lookup.SelectedInvoice.Id);
    }
}
