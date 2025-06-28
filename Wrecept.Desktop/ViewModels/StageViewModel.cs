using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Desktop;

namespace Wrecept.Desktop.ViewModels;

public partial class StageViewModel : ObservableObject
{
    public InvoiceEditorViewModel Editor { get; } = new(ServiceLocator.InvoiceService);
    public SupplierLookupViewModel SupplierLookup { get; } = new();

    [ObservableProperty]
    private bool showEditor;

    [ObservableProperty]
    private bool showSupplierLookup;

    public StageViewModel()
    {
    }
}
