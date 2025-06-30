using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Wrecept.Wpf.ViewModels;

public enum StageMenuAction
{
    InboundDeliveryNotes,
    UpdateInboundInvoices,
    EditProducts,
    EditProductGroups,
    EditSuppliers,
    EditVatKeys,
    EditPaymentMethods,
    ListProducts,
    ListSuppliers,
    ListInvoices,
    InventoryCard,
    CheckFiles,
    AfterPowerOutage,
    ScreenSettings,
    PrinterSettings,
    UserInfo,
    ExitApplication
}

public partial class StageViewModel : ObservableObject
{
    [ObservableProperty]
    private object? currentViewModel;

    private readonly InvoiceEditorViewModel _invoiceEditor;
    private readonly ProductMasterViewModel _productMaster;
    private readonly ProductGroupMasterViewModel _productGroupMaster;
    private readonly SupplierMasterViewModel _supplierMaster;
    private readonly TaxRateMasterViewModel _taxRateMaster;
    private readonly PaymentMethodMasterViewModel _paymentMethodMaster;
    private readonly AboutViewModel _about;
    private readonly PlaceholderViewModel _placeholder;
    private readonly StatusBarViewModel _statusBar;

    public StatusBarViewModel StatusBar => _statusBar;

    public StageViewModel(
        InvoiceEditorViewModel invoiceEditor,
        ProductMasterViewModel productMaster,
        ProductGroupMasterViewModel productGroupMaster,
        SupplierMasterViewModel supplierMaster,
        TaxRateMasterViewModel taxRateMaster,
        PaymentMethodMasterViewModel paymentMethodMaster,
        AboutViewModel about,
        PlaceholderViewModel placeholder,
        StatusBarViewModel statusBar)
    {
        _invoiceEditor = invoiceEditor;
        _productMaster = productMaster;
        _productGroupMaster = productGroupMaster;
        _supplierMaster = supplierMaster;
        _taxRateMaster = taxRateMaster;
        _paymentMethodMaster = paymentMethodMaster;
        _about = about;
        _placeholder = placeholder;
        _statusBar = statusBar;
        CurrentViewModel = _invoiceEditor;
        _statusBar.ActiveMenu = "Főmenü";
    }

    [RelayCommand]
    private void HandleMenu(StageMenuAction action)
    {
        _statusBar.ActiveMenu = action.ToString();
        switch (action)
        {
            case StageMenuAction.EditProducts:
            case StageMenuAction.ListProducts:
                CurrentViewModel = _productMaster;
                _statusBar.Message = "Termék nézet megnyitva";
                break;
            case StageMenuAction.EditSuppliers:
            case StageMenuAction.ListSuppliers:
                CurrentViewModel = _supplierMaster;
                _statusBar.Message = "Szállító nézet megnyitva";
                break;
            case StageMenuAction.EditProductGroups:
                CurrentViewModel = _productGroupMaster;
                _statusBar.Message = "Termékcsoport nézet megnyitva";
                break;
            case StageMenuAction.EditVatKeys:
                CurrentViewModel = _taxRateMaster;
                _statusBar.Message = "Adókulcs nézet megnyitva";
                break;
            case StageMenuAction.EditPaymentMethods:
                CurrentViewModel = _paymentMethodMaster;
                _statusBar.Message = "Fizetési mód nézet megnyitva";
                break;
            case StageMenuAction.InboundDeliveryNotes:
            case StageMenuAction.UpdateInboundInvoices:
            case StageMenuAction.ListInvoices:
            case StageMenuAction.InventoryCard:
            case StageMenuAction.CheckFiles:
            case StageMenuAction.AfterPowerOutage:
            case StageMenuAction.ScreenSettings:
            case StageMenuAction.PrinterSettings:
            case StageMenuAction.UserInfo:
                if (action == StageMenuAction.UserInfo)
                {
                    CurrentViewModel = _about;
                    _statusBar.Message = "Névjegy megjelenítve";
                }
                else
                {
                    CurrentViewModel = _placeholder;
                    _statusBar.Message = "Funkció még nincs kész";
                }
                break;
            case StageMenuAction.ExitApplication:
                Application.Current.Shutdown();
                break;
            default:
                CurrentViewModel = _invoiceEditor;
                _statusBar.Message = string.Empty;
                break;
        }
    }
}
