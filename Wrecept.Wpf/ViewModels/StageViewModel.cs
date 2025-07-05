using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Wpf.Resources;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Services;

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
    EditUnits,
    ListProducts,
    ListSuppliers,
    ListInvoices,
    InventoryCard,
    CheckFiles,
    AfterPowerOutage,
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
        ISessionService session)
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
        CurrentViewModel = _invoiceEditor;
        _statusBar.ActiveMenu = "Főmenü";
    }

[RelayCommand]
private async Task HandleMenu(StageMenuAction action)
    {
        _statusBar.ActiveMenu = action.ToString();
        switch (action)
        {
            case StageMenuAction.EditProducts:
            case StageMenuAction.ListProducts:
                CurrentViewModel = _productMaster;
                _statusBar.Message = Resources.Strings.Stage_ProductViewOpened;
                break;
            case StageMenuAction.EditSuppliers:
            case StageMenuAction.ListSuppliers:
                CurrentViewModel = _supplierMaster;
                _statusBar.Message = Resources.Strings.Stage_SupplierViewOpened;
                break;
            case StageMenuAction.EditProductGroups:
                CurrentViewModel = _productGroupMaster;
                _statusBar.Message = Resources.Strings.Stage_ProductGroupViewOpened;
                break;
            case StageMenuAction.EditVatKeys:
                CurrentViewModel = _taxRateMaster;
                _statusBar.Message = Resources.Strings.Stage_TaxRateViewOpened;
                break;
            case StageMenuAction.EditPaymentMethods:
                CurrentViewModel = _paymentMethodMaster;
                _statusBar.Message = Resources.Strings.Stage_PaymentMethodViewOpened;
                break;
            case StageMenuAction.EditUnits:
                CurrentViewModel = _unitMaster;
                _statusBar.Message = Resources.Strings.Stage_UnitViewOpened;
                break;
            case StageMenuAction.InboundDeliveryNotes:
            case StageMenuAction.UpdateInboundInvoices:
                CurrentViewModel = _invoiceEditor;
                _statusBar.Message = Resources.Strings.Stage_InvoiceEditorOpened;
                break;
            case StageMenuAction.ListInvoices:
            case StageMenuAction.InventoryCard:
            case StageMenuAction.PrinterSettings:
                CurrentViewModel = _placeholder;
                _statusBar.Message = Resources.Strings.Stage_FunctionNotReady;
                break;
            case StageMenuAction.CheckFiles:
                CurrentViewModel = _placeholder;
                _statusBar.Message = Resources.Strings.Stage_FunctionNotReady;
                var ok = await _dbHealth.CheckAsync();
                _statusBar.Message = ok ? Resources.Strings.Stage_DbCheckOk : Resources.Strings.Stage_DbCheckFailed;
                break;
            case StageMenuAction.AfterPowerOutage:
                var last = await _session.LoadLastInvoiceIdAsync();
                if (last.HasValue)
                {
                    CurrentViewModel = _invoiceEditor;
                    await _invoiceEditor.LoadInvoice(last.Value);
                    _statusBar.Message = Resources.Strings.Stage_LastInvoiceRestored;
                }
                else
                {
                    CurrentViewModel = _placeholder;
                    _statusBar.Message = Resources.Strings.Stage_NoInvoiceToRestore;
                }
                break;
            case StageMenuAction.ScreenSettings:
                var win = App.Provider.GetRequiredService<ScreenModeWindow>();
                win.Owner = App.Current.MainWindow;
                win.ShowDialog();
                _statusBar.Message = "Képernyő mód frissítve";
                break;
            case StageMenuAction.EditUserInfo:
                _statusBar.Message = Resources.Strings.Stage_UserInfoEditOpened;
                await _userInfo.LoadAsync();

                var editorVm = App.Provider.GetRequiredService<UserInfoEditorViewModel>();
                editorVm.CompanyName = _userInfo.CompanyName;
                editorVm.Address = _userInfo.Address;
                editorVm.Phone = _userInfo.Phone;
                editorVm.Email = _userInfo.Email;
                editorVm.TaxNumber = _userInfo.TaxNumber;
                editorVm.BankAccount = _userInfo.BankAccount;

                var editorWin = App.Provider.GetRequiredService<UserInfoWindow>();
                editorWin.DataContext = editorVm;
                editorVm.OnOk = _ => { editorWin.DialogResult = true; editorWin.Close(); };
                editorVm.OnCancel = () => { editorWin.DialogResult = false; editorWin.Close(); };

                if (NavigationService.ShowCenteredDialog(editorWin))
                {
                    var svc = App.Provider.GetRequiredService<IUserInfoService>();
                    await svc.SaveAsync(new UserInfo
                    {
                        CompanyName = editorVm.CompanyName,
                        Address = editorVm.Address,
                        Phone = editorVm.Phone,
                        Email = editorVm.Email,
                        TaxNumber = editorVm.TaxNumber,
                        BankAccount = editorVm.BankAccount
                    });

                    _userInfo.CompanyName = editorVm.CompanyName;
                    _userInfo.Address = editorVm.Address;
                    _userInfo.Phone = editorVm.Phone;
                    _userInfo.Email = editorVm.Email;
                    _userInfo.TaxNumber = editorVm.TaxNumber;
                    _userInfo.BankAccount = editorVm.BankAccount;
                }

                break;
            case StageMenuAction.UserInfo:
                CurrentViewModel = _about;
                _statusBar.Message = Resources.Strings.Stage_AboutOpened;
                await _about.LoadAsync();
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
