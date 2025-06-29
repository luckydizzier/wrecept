using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Services;
using Wrecept.Core.Repositories;

namespace Wrecept.Desktop.ViewModels;

public partial class StageViewModel : ObservableObject
{
    public InvoiceEditorViewModel Editor { get; }
    public SupplierLookupViewModel SupplierLookup { get; }
    public ProductViewModel Product { get; }
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
    private bool showProduct;

    [ObservableProperty]
    private bool showProductGroup;

    [ObservableProperty]
    private bool showTaxRate;

    [ObservableProperty]
    private bool showPaymentMethod;

    public StageViewModel(IInvoiceService invoiceService, IProductService productService, ISupplierRepository supplierRepository)
    {
        Editor = new InvoiceEditorViewModel(invoiceService);
        Product = new ProductViewModel(productService);
        SupplierLookup = new SupplierLookupViewModel(supplierRepository);
        ProductGroup = new ProductGroupViewModel();
        TaxRate = new TaxRateViewModel();
        PaymentMethod = new PaymentMethodViewModel();
    }

    public void OpenInvoiceEditor()
    {
        HideAll();
        ShowEditor = true;
    }

    public void OpenProductView()
    {
        HideAll();
        ShowProduct = true;
    }

    public void OpenProductGroupView()
    {
        HideAll();
        ShowProductGroup = true;
    }

    public void OpenSupplierLookupView()
    {
        HideAll();
        ShowSupplierLookup = true;
    }

    public void OpenTaxRateView()
    {
        HideAll();
        ShowTaxRate = true;
    }

    public void OpenPaymentMethodView()
    {
        HideAll();
        ShowPaymentMethod = true;
    }

    public void HideAll()
    {
        ShowEditor = false;
        ShowProduct = false;
        ShowSupplierLookup = false;
        ShowProductGroup = false;
        ShowTaxRate = false;
        ShowPaymentMethod = false;
        IsSubMenuOpen = false;
    }

    partial void OnShowEditorChanged(bool value)
    {
        Debug.WriteLine($"ShowEditor set to {value}");
    }

    partial void OnShowSupplierLookupChanged(bool value)
    {
        Debug.WriteLine($"ShowSupplierLookup set to {value}");
    }

    partial void OnShowProductChanged(bool value)
    {
        Debug.WriteLine($"ShowProduct set to {value}");
        if (value)
            _ = Product.LoadAsync();
    }

    partial void OnShowProductGroupChanged(bool value)
    {
        Debug.WriteLine($"ShowProductGroup set to {value}");
    }

    partial void OnShowTaxRateChanged(bool value)
    {
        Debug.WriteLine($"ShowTaxRate set to {value}");
    }

    partial void OnShowPaymentMethodChanged(bool value)
    {
        Debug.WriteLine($"ShowPaymentMethod set to {value}");
    }
}
