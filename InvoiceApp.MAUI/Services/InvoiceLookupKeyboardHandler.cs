using Microsoft.Maui.Controls;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.MAUI.Services;

public class InvoiceLookupKeyboardHandler : IKeyboardHandler
{
    private readonly InvoiceLookupViewModel _viewModel;

    public InvoiceLookupKeyboardHandler(InvoiceLookupViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        if (e.Key == Key.Insert || e.Key == Key.Up)
        {
            _viewModel.CreateNewInvoiceCommand.Execute(null);
            return true;
        }
        return false;
    }
}
