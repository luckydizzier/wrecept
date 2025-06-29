using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
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

    private readonly string[][] _submenuItems =
    {
        new[] { "Bejövő számlák kezelése", "Bejövő számlák aktualizálása" },
        new[] { "Termékek", "Termékcsoportok", "Szállítók", "ÁFA-kulcsok", "Fizetési módok" },
        new[] { "Terméklista", "Szállítók listája", "Számlák listája", "Készletkarton" },
        new[] { "Állományok ellenőrzése", "Áramszünet után", "Képernyő beállítása", "Nyomtató beállítás" },
        new[] { "A program felhasználójának adatai" },
        new[] { "Kilépés" }
    };

    [ObservableProperty]
    private ObservableCollection<string> currentSubmenuItems = new();

    public StageViewModel(IInvoiceService invoiceService, IProductService productService, ISupplierRepository supplierRepository)
    {
        Editor = new InvoiceEditorViewModel(invoiceService);
        Product = new ProductViewModel(productService);
        SupplierLookup = new SupplierLookupViewModel(supplierRepository);
        ProductGroup = new ProductGroupViewModel();
        TaxRate = new TaxRateViewModel();
        PaymentMethod = new PaymentMethodViewModel();

        UpdateCurrentSubmenuItems(0);
    }

    partial void OnSelectedIndexChanged(int value)
    {
        UpdateCurrentSubmenuItems(value);
    }

    private void UpdateCurrentSubmenuItems(int index)
    {
        CurrentSubmenuItems.Clear();
        if (index >= 0 && index < _submenuItems.Length)
        {
            foreach (var item in _submenuItems[index])
                CurrentSubmenuItems.Add(item);
        }
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

    partial void OnShowProductChanged(bool value)
    {
        if (value)
            _ = Product.LoadAsync();
    }
}
