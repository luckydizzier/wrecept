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
    [ObservableProperty]
    private string product = string.Empty;

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

    public InvoiceEditorViewModel(IPaymentMethodService paymentMethods, ITaxRateService taxRates, ISupplierService suppliers)
    {
        _paymentMethods = paymentMethods;
        _taxRates = taxRates;
        _suppliers = suppliers;
        Items = new ObservableCollection<InvoiceItemRowViewModel>(
            Enumerable.Range(1, 3).Select(_ => new InvoiceItemRowViewModel()));
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
    }
}
