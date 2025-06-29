using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Services;

namespace Wrecept.Desktop.ViewModels;

public partial class StageViewModel : ObservableObject
{
    public InvoiceEditorViewModel Editor { get; }
    public SupplierLookupViewModel SupplierLookup { get; }
    public ProductGroupViewModel ProductGroup { get; }
    public TaxRateViewModel TaxRate { get; }
    public PaymentMethodViewModel PaymentMethod { get; }

    [ObservableProperty]
    private int selectedIndex;

    [ObservableProperty]
    private int selectedSubmenuIndex;

    [ObservableProperty]
    private bool isSubMenuOpen;

    [ObservableProperty]
    private bool showEditor;

    [ObservableProperty]
    private bool showSupplierLookup;

    [ObservableProperty]
    private bool showProductGroup;

    [ObservableProperty]
    private bool showTaxRate;

    [ObservableProperty]
    private bool showPaymentMethod;

    public StageViewModel(IInvoiceService invoiceService)
    {
        Editor = new InvoiceEditorViewModel(invoiceService);
        SupplierLookup = new SupplierLookupViewModel();
        ProductGroup = new ProductGroupViewModel();
        TaxRate = new TaxRateViewModel();
        PaymentMethod = new PaymentMethodViewModel();
    }

    public void HideAll()
    {
        ShowEditor = false;
        ShowSupplierLookup = false;
        ShowProductGroup = false;
        ShowTaxRate = false;
        ShowPaymentMethod = false;
        IsSubMenuOpen = false;
    }
}
