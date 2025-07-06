using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Wpf.ViewModels;

public partial class InvoiceCreatePromptViewModel : ObservableObject
{
    private readonly InvoiceLookupViewModel _parent;

    [ObservableProperty]
    private string editableNumber = string.Empty;

    public string Number { get; }

    public InvoiceCreatePromptViewModel(InvoiceLookupViewModel parent, string number)
    {
        _parent = parent;
        Number = number;
        EditableNumber = number;
    }

    public string Message => $"\u00daj sz\u00e1mla '{EditableNumber}'? (Enter=Igen / Esc=Nem)";

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        await _parent.CreateInvoiceAsync(EditableNumber);
        _parent.InlinePrompt = null;
    }

    [RelayCommand]
    private void Cancel()
    {
        _parent.InlinePrompt = null;
    }
}
