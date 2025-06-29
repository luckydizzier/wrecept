using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.ViewModels;

public partial class StageViewModel : ObservableObject
{
    [ObservableProperty]
    private object? currentViewModel;

    private readonly InvoiceEditorViewModel _invoiceEditor;
    private readonly ProductMasterViewModel _productMaster;
    private readonly SupplierMasterViewModel _supplierMaster;

    public StageViewModel(
        InvoiceEditorViewModel invoiceEditor,
        ProductMasterViewModel productMaster,
        SupplierMasterViewModel supplierMaster)
    {
        _invoiceEditor = invoiceEditor;
        _productMaster = productMaster;
        _supplierMaster = supplierMaster;
        CurrentViewModel = _invoiceEditor;
    }

    [RelayCommand]
    private void ShowInvoiceEditor() => CurrentViewModel = _invoiceEditor;

    [RelayCommand]
    private void ShowProductMaster() => CurrentViewModel = _productMaster;

    [RelayCommand]
    private void ShowSupplierMaster() => CurrentViewModel = _supplierMaster;
}
