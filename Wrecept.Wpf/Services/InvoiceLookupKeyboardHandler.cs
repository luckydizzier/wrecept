using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.ViewModels;
namespace Wrecept.Wpf.Services;

public class InvoiceLookupKeyboardHandler : IKeyboardHandler
{
    private readonly InvoiceLookupViewModel _viewModel;

    public InvoiceLookupKeyboardHandler(InvoiceLookupViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        if (e.Key != Key.Insert && e.Key != Key.Up)
            return false;

        if (Keyboard.FocusedElement is not DependencyObject element)
            return false;

        var list = GetInvoiceList(element);
        if (list is null)
            return false;

        if (list.Items.Count == 0 || list.SelectedIndex <= 0)
        {
            _ = _viewModel.PromptNewInvoiceAsync();
            return true;
        }

        return false;
    }

    private static ListBox? GetInvoiceList(DependencyObject element)
    {
        if (element is ListBox list && list.Name == "InvoiceList")
            return list;

        if (element is ListBoxItem item)
        {
            var parent = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
            if (parent?.Name == "InvoiceList")
                return parent;
        }

        return null;
    }
}
