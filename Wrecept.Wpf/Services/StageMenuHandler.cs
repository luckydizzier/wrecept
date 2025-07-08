using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using Wrecept.Core.Entities;
using Wrecept.Core.Enums;
using Wrecept.Core.Services;
using Wrecept.Wpf.Resources;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;

namespace Wrecept.Wpf.Services;

public class StageMenuHandler
{
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
    private readonly Dictionary<StageMenuAction, Func<StageViewModel, Task>> _handlers;

    public StageMenuHandler(
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
        AppStateService state)
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

        _handlers = new()
        {
            [StageMenuAction.EditProducts] = HandleProductViewAsync,
            [StageMenuAction.ListProducts] = HandleProductViewAsync,
            [StageMenuAction.EditSuppliers] = HandleSupplierViewAsync,
            [StageMenuAction.ListSuppliers] = HandleSupplierViewAsync,
            [StageMenuAction.EditProductGroups] = HandleProductGroupViewAsync,
            [StageMenuAction.EditVatKeys] = HandleTaxRateViewAsync,
            [StageMenuAction.EditPaymentMethods] = HandlePaymentMethodViewAsync,
            [StageMenuAction.EditUnits] = HandleUnitViewAsync,
            [StageMenuAction.InboundDeliveryNotes] = HandleInvoiceEditorAsync,
            [StageMenuAction.UpdateInboundInvoices] = HandleInvoiceEditorAsync,
            [StageMenuAction.ListInvoices] = HandleNotReadyAsync,
            [StageMenuAction.InventoryCard] = HandleNotReadyAsync,
            [StageMenuAction.PrinterSettings] = HandleNotReadyAsync,
            [StageMenuAction.CheckFiles] = HandleCheckFilesAsync,
            [StageMenuAction.AfterPowerOutage] = HandleRestoreInvoiceAsync,
            [StageMenuAction.BackupData] = HandleBackupAsync,
            [StageMenuAction.RestoreData] = HandleRestoreDataAsync,
            [StageMenuAction.ScreenSettings] = HandleScreenSettingsAsync,
            [StageMenuAction.EditUserInfo] = HandleEditUserInfoAsync,
            [StageMenuAction.UserInfo] = HandleUserInfoAsync,
            [StageMenuAction.ExitApplication] = HandleExitAsync
        };
    }

    public async Task HandleAsync(StageViewModel stage, StageMenuAction action)
    {
        _statusBar.ActiveMenu = action.ToString();
        _state.LastView = action;
        if (!_handlers.TryGetValue(action, out var handler))
            handler = HandleDefaultAsync;

        await handler(stage);
        await _state.SaveAsync();
    }

    private Task HandleProductViewAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _productMaster;
        _statusBar.Message = Strings.Stage_ProductViewOpened;
        _state.InteractionState = AppInteractionState.EditingMasterData;
        return Task.CompletedTask;
    }

    private Task HandleSupplierViewAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _supplierMaster;
        _statusBar.Message = Strings.Stage_SupplierViewOpened;
        _state.InteractionState = AppInteractionState.EditingMasterData;
        return Task.CompletedTask;
    }

    private Task HandleProductGroupViewAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _productGroupMaster;
        _statusBar.Message = Strings.Stage_ProductGroupViewOpened;
        _state.InteractionState = AppInteractionState.EditingMasterData;
        return Task.CompletedTask;
    }

    private Task HandleTaxRateViewAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _taxRateMaster;
        _statusBar.Message = Strings.Stage_TaxRateViewOpened;
        _state.InteractionState = AppInteractionState.EditingMasterData;
        return Task.CompletedTask;
    }

    private Task HandlePaymentMethodViewAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _paymentMethodMaster;
        _statusBar.Message = Strings.Stage_PaymentMethodViewOpened;
        _state.InteractionState = AppInteractionState.EditingMasterData;
        return Task.CompletedTask;
    }

    private Task HandleUnitViewAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _unitMaster;
        _statusBar.Message = Strings.Stage_UnitViewOpened;
        _state.InteractionState = AppInteractionState.EditingMasterData;
        return Task.CompletedTask;
    }

    private Task HandleInvoiceEditorAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _invoiceEditor;
        _statusBar.Message = Strings.Stage_InvoiceEditorOpened;
        _state.InteractionState = AppInteractionState.EditingInvoice;
        return Task.CompletedTask;
    }

    private Task HandleNotReadyAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _placeholder;
        _statusBar.Message = Strings.Stage_FunctionNotReady;
        _state.InteractionState = AppInteractionState.BrowsingInvoices;
        return Task.CompletedTask;
    }

    private async Task HandleCheckFilesAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _placeholder;
        _statusBar.Message = Strings.Stage_FunctionNotReady;
        var ok = await _dbHealth.CheckAsync();
        _statusBar.Message = ok ? Strings.Stage_DbCheckOk : Strings.Stage_DbCheckFailed;
        _state.InteractionState = AppInteractionState.MainMenu;
    }

    private async Task HandleRestoreInvoiceAsync(StageViewModel stage)
    {
        var last = await _session.LoadLastInvoiceIdAsync();
        if (last.HasValue)
        {
            stage.CurrentViewModel = _invoiceEditor;
            await _invoiceEditor.LoadInvoice(last.Value);
            _statusBar.Message = Strings.Stage_LastInvoiceRestored;
            _state.CurrentInvoiceId = last.Value;
            _state.InteractionState = AppInteractionState.EditingInvoice;
        }
        else
        {
            stage.CurrentViewModel = _placeholder;
            _statusBar.Message = Strings.Stage_NoInvoiceToRestore;
            _state.InteractionState = AppInteractionState.MainMenu;
        }
    }

    private async Task HandleBackupAsync(StageViewModel stage)
    {
        var saveDlg = new SaveFileDialog
        {
            Filter = "ZIP (*.zip)|*.zip|All files|*.*",
            FileName = "wrecept-backup.zip",
            InitialDirectory = Path.GetDirectoryName(App.DbPath)
        };
        if (NavigationService.ShowFileDialog(saveDlg))
        {
            var svc = App.Provider.GetRequiredService<IBackupService>();
            await svc.BackupAsync(saveDlg.FileName);
            _statusBar.Message = Strings.Stage_BackupSuccess;
        }
    }

    private async Task HandleRestoreDataAsync(StageViewModel stage)
    {
        var openDlg = new OpenFileDialog
        {
            Filter = "ZIP (*.zip)|*.zip|All files|*.*",
            InitialDirectory = Path.GetDirectoryName(App.DbPath)
        };
        if (NavigationService.ShowFileDialog(openDlg))
        {
            var svc = App.Provider.GetRequiredService<IBackupService>();
            await svc.RestoreAsync(openDlg.FileName);
            _statusBar.Message = Strings.Stage_RestoreSuccess;
        }
    }

    private async Task HandleScreenSettingsAsync(StageViewModel stage)
    {
        var win = App.Provider.GetRequiredService<ScreenModeWindow>();
        win.Owner = App.Current.MainWindow;
        win.ShowDialog();
        _statusBar.Message = "Képernyő mód frissítve";
        await Task.CompletedTask;
    }

    private async Task HandleEditUserInfoAsync(StageViewModel stage)
    {
        _statusBar.Message = Strings.Stage_UserInfoEditOpened;
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
    }

    private async Task HandleUserInfoAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _about;
        _statusBar.Message = Strings.Stage_AboutOpened;
        await _about.LoadAsync();
    }

    private Task HandleExitAsync(StageViewModel stage)
    {
        Application.Current.Shutdown();
        _state.InteractionState = AppInteractionState.Exiting;
        return Task.CompletedTask;
    }

    private Task HandleDefaultAsync(StageViewModel stage)
    {
        stage.CurrentViewModel = _invoiceEditor;
        _statusBar.Message = string.Empty;
        _state.InteractionState = AppInteractionState.MainMenu;
        return Task.CompletedTask;
    }
}
