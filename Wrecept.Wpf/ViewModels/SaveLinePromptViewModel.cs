using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.ViewModels;

public partial class SaveLinePromptViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;

    public SaveLinePromptViewModel(InvoiceEditorViewModel parent)
    {
        _parent = parent;
    }

    public string Message => "Mentsem ezt a sort? (Enter=Igen, Esc=Nem)";

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        await _parent.AddLineItemAsync();
        _parent.SavePrompt = null;
    }

    [RelayCommand]
    private void Cancel()
    {
        _parent.SavePrompt = null;
    }
}

