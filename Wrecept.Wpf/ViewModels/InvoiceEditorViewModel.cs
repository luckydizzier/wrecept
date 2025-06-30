using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

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
}

public partial class InvoiceEditorViewModel : ObservableObject
{
    public ObservableCollection<InvoiceItemRowViewModel> Items { get; }

    public ObservableCollection<PaymentMethod> PaymentMethods { get; } = new();
    public ObservableCollection<TaxRate> TaxRates { get; } = new();
    public ObservableCollection<Supplier> Suppliers { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<Unit> Units { get; } = new();

    [ObservableProperty]
    private string supplier = string.Empty;

    [ObservableProperty]
    private Guid supplierId;

    [ObservableProperty]
    private DateOnly? invoiceDate;

    [ObservableProperty]
    private string number = string.Empty;

    [ObservableProperty]
    private Guid paymentMethodId;

    [ObservableProperty]
    private bool isGross;

    private readonly IPaymentMethodService _paymentMethods;
    private readonly ITaxRateService _taxRates;
    private readonly ISupplierService _suppliers;
    private readonly IProductService _productsService;
    private readonly IUnitService _unitsService;

    [ObservableProperty]
    private object? inlineCreator;

    public InvoiceEditorViewModel(
        IPaymentMethodService paymentMethods,
        ITaxRateService taxRates,
        ISupplierService suppliers,
        IProductService products,
        IUnitService units)
    {
        _paymentMethods = paymentMethods;
        _taxRates = taxRates;
        _suppliers = suppliers;
        _productsService = products;
        _unitsService = units;
        Items = new ObservableCollection<InvoiceItemRowViewModel>(
            Enumerable.Range(1, 3).Select(_ => new InvoiceItemRowViewModel(this)));
    }

    public async Task LoadAsync()
    {
        var methods = await _paymentMethods.GetActiveAsync();
        PaymentMethods.Clear();
        foreach (var m in methods)
            PaymentMethods.Add(m);

        var supplierItems = await _suppliers.GetActiveAsync();
        Suppliers.Clear();
        foreach (var s in supplierItems)
            Suppliers.Add(s);

        var taxRates = await _taxRates.GetActiveAsync(DateTime.UtcNow);
        TaxRates.Clear();
        foreach (var t in taxRates)
            TaxRates.Add(t);

        var productItems = await _productsService.GetActiveAsync();
        Products.Clear();
        foreach (var p in productItems)
            Products.Add(p);

        var unitItems = await _unitsService.GetActiveAsync();
        Units.Clear();
        foreach (var u in unitItems)
            Units.Add(u);
    }

    public async Task CheckProductAsync(InvoiceItemRowViewModel row, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

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
        }
    }
}
