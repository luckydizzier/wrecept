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
                _vm.AddLineItemCommand.Execute(null);
                return true;
            case Key.Delete:
                _vm.ShowArchivePromptCommand.Execute(null);
                return true;
            case Key.Enter:
                _vm.SaveEditedItemCommand.Execute(null);
                return true;
            case Key.Escape:
                _vm.CloseCommand.Execute(null);
                return true;
        }
        return false;
    }
}
