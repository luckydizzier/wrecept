using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Desktop;
using Wrecept.Desktop.Models;

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
    private int selectedSubmenuIndex;

    [ObservableProperty]
    private ObservableCollection<SubmenuItem> currentSubmenuItems = new();

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

    private readonly Dictionary<int, List<SubmenuItem>> _submenuMap;

    public StageViewModel()
    {
        _submenuMap = new()
        {
            [0] = new()
            {
                new SubmenuItem(0, "Bejövő szállítólevelek", () => { }),
                new SubmenuItem(1, "Bejövő számlák aktualizálása", () => { })
            },
            [1] = new()
            {
                new SubmenuItem(0, "Termékek", () => { }),
                new SubmenuItem(1, "Termékcsoportok", () => ShowProductGroup = true),
                new SubmenuItem(2, "Szállítók", () => { }),
                new SubmenuItem(3, "ÁFA-kulcsok", () => ShowTaxRate = true),
                new SubmenuItem(4, "Fizetési módok", () => ShowPaymentMethod = true)
            },
            [2] = new()
            {
                new SubmenuItem(0, "Terméklista", () => { }),
                new SubmenuItem(1, "Szállítók listája", () => { }),
                new SubmenuItem(2, "Számlák listája", () => { }),
                new SubmenuItem(3, "Készletkarton", () => { })
            },
            [3] = new()
            {
                new SubmenuItem(0, "Állományok ellenőrzése", () => { }),
                new SubmenuItem(1, "Áramszünet után", () => { }),
                new SubmenuItem(2, "Képernyő beállítása", () => { }),
                new SubmenuItem(3, "Nyomtató beállítás", () => { })
            },
            [4] = new()
            {
                new SubmenuItem(0, "A program felhasználójának adatai", () => { })
            },
            [5] = new()
            {
                new SubmenuItem(0, "Kilépés", () => { })
            }
        };
        LoadSubmenu(0);
    }

    private void LoadSubmenu(int mainIndex)
    {
        CurrentSubmenuItems.Clear();
        if (_submenuMap.TryGetValue(mainIndex, out var list))
        {
            foreach (var item in list)
                CurrentSubmenuItems.Add(item);
        }
        SelectedSubmenuIndex = 0;
    }

    partial void OnSelectedIndexChanged(int value)
    {
        LoadSubmenu(value);
    }

    public void ExecuteCurrentSubmenu()
    {
        HideAll();
        if (_submenuMap.TryGetValue(SelectedIndex, out var list) &&
            SelectedSubmenuIndex >= 0 && SelectedSubmenuIndex < list.Count)
        {
            list[SelectedSubmenuIndex].Action.Invoke();
        }
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
