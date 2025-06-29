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
    private ObservableObject? activeViewModel;

    public StageViewModel(IInvoiceService invoiceService, IProductService productService, ISupplierRepository supplierRepository)
    {
        Editor = new InvoiceEditorViewModel(invoiceService);
        Product = new ProductViewModel(productService);
        SupplierLookup = new SupplierLookupViewModel(supplierRepository);
        ProductGroup = new ProductGroupViewModel();
        TaxRate = new TaxRateViewModel();
        PaymentMethod = new PaymentMethodViewModel();
    }

    public void OpenInvoiceEditor() => Activate(Editor);

    public void OpenProductView() => Activate(Product);

    public void OpenProductGroupView() => Activate(ProductGroup);

    public void OpenSupplierLookupView() => Activate(SupplierLookup);

    public void OpenTaxRateView() => Activate(TaxRate);

    public void OpenPaymentMethodView() => Activate(PaymentMethod);

    private void Activate(ObservableObject viewModel)
    {
        ActiveViewModel = viewModel;
        if (viewModel == Product)
            _ = Product.LoadAsync();
        IsSubMenuOpen = false;
    }
}
