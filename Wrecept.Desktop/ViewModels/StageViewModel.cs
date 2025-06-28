using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Desktop;

namespace Wrecept.Desktop.ViewModels;

public partial class StageViewModel : ObservableObject
{
    public InvoiceEditorViewModel Editor { get; } = new(ServiceLocator.InvoiceService);
    public SupplierLookupViewModel SupplierLookup { get; } = new();
    public ProductGroupViewModel ProductGroup { get; } = new();
    public TaxRateViewModel TaxRate { get; } = new();
    public PaymentMethodViewModel PaymentMethod { get; } = new();

    [ObservableProperty]
    private int selectedIndex;

    [ObservableProperty]
    private int selectedSubIndex;

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

    public StageViewModel()
    {
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
