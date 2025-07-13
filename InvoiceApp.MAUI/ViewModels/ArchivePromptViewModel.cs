using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.MAUI.ViewModels;

public partial class ArchivePromptViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;

    public ArchivePromptViewModel(InvoiceEditorViewModel parent)
    {
        _parent = parent;
    }

    public string Message => "Ez a művelet véglegesíti a számlát. A továbbiakban nem módosítható. Biztosan folytatod? (Enter=Igen, Esc=Mégsem)";

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        await _parent.ArchiveAsync();
        _parent.ArchivePrompt = null;
    }

    [RelayCommand]
    private void Cancel()
    {
        _parent.ArchivePrompt = null;
    }
}

