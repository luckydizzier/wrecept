using System;
using Microsoft.Maui.Controls;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.MAUI.Services;

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
                _vm.ItemsEditor.AddLineItemCommand.Execute(null);
                return true;
            case Key.Delete:
                _vm.ShowArchivePromptCommand.Execute(null);
                return true;
            case Key.Enter or Key.Return:
                _vm.ItemsEditor.SaveEditedItemCommand.Execute(null);
                return true;
            case Key.Escape:
                _vm.CloseCommand.Execute(null);
                return true;
        }
        return false;
    }
}
