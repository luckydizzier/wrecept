using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Services;

public class InvoiceEditorKeyboardHandler : IKeyboardHandler
{
    private readonly InvoiceEditorViewModel _vm;

    public InvoiceEditorKeyboardHandler(InvoiceEditorViewModel vm)
    {
        _vm = vm;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Insert:
                if (Keyboard.FocusedElement is DependencyObject element &&
                    GetInvoiceList(element) is not null)
                {
                    _ = _vm.Lookup.PromptNewInvoiceAsync();
                }
                else
                {
                    _vm.AddLineItemCommand.Execute(null);
                }
                return true;
            case Key.Delete:
                _vm.ShowArchivePromptCommand.Execute(null);
                return true;
            case Key.Enter or Key.Return:
                _vm.SaveEditedItemCommand.Execute(null);
                return true;
            case Key.Escape:
                _vm.CloseCommand.Execute(null);
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
