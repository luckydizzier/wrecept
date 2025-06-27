using CommunityToolkit.Mvvm.ComponentModel;

namespace Wrecept.Desktop.ViewModels;

public partial class StageViewModel : ObservableObject
{
    public MainMenuViewModel Menu { get; } = new();
    public InvoiceEditorViewModel Editor { get; } = new();
    public SupplierLookupViewModel SupplierLookup { get; } = new();

    [ObservableProperty]
    private bool showEditor;

    [ObservableProperty]
    private bool showSupplierLookup;

    public StageViewModel()
    {
        Menu.ItemActivated += idx =>
        {
            ShowEditor = idx == 0;
            ShowSupplierLookup = idx == 2;
        };
    }
}
