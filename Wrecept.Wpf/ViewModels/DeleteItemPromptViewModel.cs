using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class DeleteItemPromptViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;
    private readonly InvoiceItemRowViewModel _row;

    public DeleteItemPromptViewModel(InvoiceEditorViewModel parent, InvoiceItemRowViewModel row)
    {
        _parent = parent;
        _row = row;
    }

    public string Message => "Biztosan törlöd ezt a tételt? (Enter=Igen, Esc=Nem)";

    [RelayCommand]
    private void Confirm()
    {
        _parent.DeleteItemConfirmed(_row);
        _parent.DeletePrompt = null;
        var tracker = App.Provider.GetRequiredService<IFocusTrackerService>();
        var focus = App.Provider.GetRequiredService<FocusManager>();
        var last = tracker.GetLast("InvoiceEditorView");
        focus.RequestFocus(last);
    }

    [RelayCommand]
    private void Cancel()
    {
        _parent.DeletePrompt = null;
        var tracker = App.Provider.GetRequiredService<IFocusTrackerService>();
        var focus = App.Provider.GetRequiredService<FocusManager>();
        var last = tracker.GetLast("InvoiceEditorView");
        focus.RequestFocus(last);
    }
}
