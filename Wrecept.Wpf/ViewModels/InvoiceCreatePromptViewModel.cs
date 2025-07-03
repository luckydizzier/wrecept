using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class InvoiceCreatePromptViewModel : ObservableObject
{
    private readonly InvoiceLookupViewModel _parent;

    public string Number { get; }

    public InvoiceCreatePromptViewModel(InvoiceLookupViewModel parent, string number)
    {
        _parent = parent;
        Number = number;
    }

    public string Message => $"\u00daj sz\u00e1mla '{Number}'? (Enter=Igen / Esc=Nem)";

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        await _parent.CreateInvoiceAsync(Number);
        _parent.InlinePrompt = null;
        var focus = App.Provider.GetRequiredService<FocusManager>();
        var last = focus.GetLast("InvoiceEditorView");
        focus.RequestFocus(last);
    }

    [RelayCommand]
    private void Cancel()
    {
        _parent.InlinePrompt = null;
        var focus = App.Provider.GetRequiredService<FocusManager>();
        var last = focus.GetLast("InvoiceEditorView");
        focus.RequestFocus(last);
    }
}
