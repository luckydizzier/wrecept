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
                    _vm.Lookup.CreateNewInvoiceCommand.Execute(null);
                }
                else
                {
                    _vm.AddLineItemCommand.Execute(null);
                }
                return true;
            case Key.Delete:
                if (Keyboard.FocusedElement is DependencyObject del && IsEntryOrGrid(del))
                {
                    _vm.ShowArchivePromptCommand.Execute(null);
                    return true;
                }
                return false;
            case Key.Enter or Key.Return:
                if (Keyboard.FocusedElement is DependencyObject enter && IsEntryOrGrid(enter))
                {
                    _vm.SaveEditedItemCommand.Execute(null);
                    return true;
                }
                return false;
            case Key.Escape:
                if (Keyboard.FocusedElement is DependencyObject esc && IsEntryOrGrid(esc))
                {
                    _vm.CloseCommand.Execute(null);
                    return true;
                }
                return false;
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

    private static bool IsEntryOrGrid(DependencyObject element)
    {
        if (element is FrameworkElement fe && fe.Name.StartsWith("Entry"))
            return true;

        return element.FindAncestor<InvoiceItemsGrid>() != null;
    }
}
