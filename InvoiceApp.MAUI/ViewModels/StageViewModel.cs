using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using InvoiceApp.Core.Enums;
using System.IO;
using InvoiceApp.Core.Entities;
using InvoiceApp.Core.Services;
using InvoiceApp.MAUI.Resources;
using InvoiceApp.MAUI.Views;
using InvoiceApp.MAUI.Services;

namespace InvoiceApp.MAUI.ViewModels;

public enum StageMenuAction
{
    InboundDeliveryNotes,
    UpdateInboundInvoices,
    EditProducts,
    EditProductGroups,
    EditSuppliers,
    EditVatKeys,
    EditPaymentMethods,
    EditUnits,
    ListProducts,
    ListSuppliers,
    ListInvoices,
    InventoryCard,
    CheckFiles,
    AfterPowerOutage,
    BackupData,
    RestoreData,
    ScreenSettings,
    PrinterSettings,
    EditUserInfo,
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
    private readonly UnitMasterViewModel _unitMaster;
    private readonly UserInfoViewModel _userInfo;
    private readonly AboutViewModel _about;
    private readonly PlaceholderViewModel _placeholder;
    private readonly StatusBarViewModel _statusBar;
    private readonly IDbHealthService _dbHealth;
    private readonly ISessionService _session;
    private readonly AppStateService _state;
    private readonly StageMenuHandler _menuHandler;

    public StatusBarViewModel StatusBar => _statusBar;
    public UserInfoViewModel UserInfo => _userInfo;

    public StageViewModel(
        InvoiceEditorViewModel invoiceEditor,
        ProductMasterViewModel productMaster,
        ProductGroupMasterViewModel productGroupMaster,
        SupplierMasterViewModel supplierMaster,
        TaxRateMasterViewModel taxRateMaster,
        PaymentMethodMasterViewModel paymentMethodMaster,
        UnitMasterViewModel unitMaster,
        UserInfoViewModel userInfo,
        AboutViewModel about,
        PlaceholderViewModel placeholder,
        StatusBarViewModel statusBar,
        IDbHealthService dbHealth,
        ISessionService session,
        AppStateService state,
        StageMenuHandler menuHandler)
    {
        _invoiceEditor = invoiceEditor;
        _productMaster = productMaster;
        _productGroupMaster = productGroupMaster;
        _supplierMaster = supplierMaster;
        _taxRateMaster = taxRateMaster;
        _paymentMethodMaster = paymentMethodMaster;
        _unitMaster = unitMaster;
        _userInfo = userInfo;
        _about = about;
        _placeholder = placeholder;
        _statusBar = statusBar;
        _dbHealth = dbHealth;
        _session = session;
        _state = state;
        _menuHandler = menuHandler;
        ApplySavedState();
        _state.InteractionState = AppInteractionState.MainMenu;
    }

    private void ApplySavedState()
    {
        _statusBar.ActiveMenu = "Főmenü";
        StageMenuAction view = _state.LastView;
        switch (view)
        {
            case StageMenuAction.EditProducts:
            case StageMenuAction.ListProducts:
                CurrentViewModel = _productMaster;
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            case StageMenuAction.EditSuppliers:
            case StageMenuAction.ListSuppliers:
                CurrentViewModel = _supplierMaster;
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            case StageMenuAction.EditProductGroups:
                CurrentViewModel = _productGroupMaster;
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            case StageMenuAction.EditVatKeys:
                CurrentViewModel = _taxRateMaster;
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            case StageMenuAction.EditPaymentMethods:
                CurrentViewModel = _paymentMethodMaster;
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            case StageMenuAction.EditUnits:
                CurrentViewModel = _unitMaster;
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            default:
                CurrentViewModel = _invoiceEditor;
                if (_state.CurrentInvoiceId.HasValue)
                    _ = _invoiceEditor.LoadInvoice(_state.CurrentInvoiceId.Value);
                _state.InteractionState = AppInteractionState.EditingInvoice;
                break;
        }
    }

[RelayCommand]
private Task HandleMenu(StageMenuAction action)
    => _menuHandler.HandleAsync(this, action);
}
