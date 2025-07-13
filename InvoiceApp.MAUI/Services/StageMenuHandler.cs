using InvoiceApp.MAUI.ViewModels;
using Wrecept.Core.Enums;

namespace InvoiceApp.MAUI.Services;

public class StageMenuHandler
{
    private readonly InvoiceEditorViewModel _invoiceEditor;
    private readonly ProductMasterViewModel _productMaster;
    private readonly SupplierMasterViewModel _supplierMaster;
    private readonly StatusBarViewModel _statusBar;
    private readonly AppStateService _state;

    public StageMenuHandler(
        InvoiceEditorViewModel invoiceEditor,
        ProductMasterViewModel productMaster,
        SupplierMasterViewModel supplierMaster,
        StatusBarViewModel statusBar,
        AppStateService state)
    {
        _invoiceEditor = invoiceEditor;
        _productMaster = productMaster;
        _supplierMaster = supplierMaster;
        _statusBar = statusBar;
        _state = state;
    }

    public Task HandleAsync(StageViewModel stage, InvoiceApp.MAUI.ViewModels.StageMenuAction action)
    {
        _state.LastView = action;
        switch (action)
        {
            case StageMenuAction.EditProducts:
                stage.CurrentViewModel = _productMaster;
                _statusBar.Message = "Termékek";
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            case StageMenuAction.EditSuppliers:
                stage.CurrentViewModel = _supplierMaster;
                _statusBar.Message = "Szállítók";
                _state.InteractionState = AppInteractionState.EditingMasterData;
                break;
            default:
                stage.CurrentViewModel = _invoiceEditor;
                _statusBar.Message = "Számlák";
                _state.InteractionState = AppInteractionState.EditingInvoice;
                break;
        }
        return Task.CompletedTask;
    }
}
