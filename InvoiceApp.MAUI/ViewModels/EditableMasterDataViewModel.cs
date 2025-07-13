using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using InvoiceApp.Core.Enums;
using InvoiceApp.MAUI.Services;

namespace InvoiceApp.MAUI.ViewModels;

public abstract partial class EditableMasterDataViewModel<T> : MasterDataBaseViewModel<T>, IEditableMasterDataViewModel
{
    [ObservableProperty]
    private bool isEditing;

    private readonly AppStateService _state;

    public IRelayCommand EditSelectedCommand { get; }
    public IRelayCommand DeleteSelectedCommand { get; }
    public IRelayCommand CloseDetailsCommand { get; }

    protected EditableMasterDataViewModel(AppStateService state)
    {
        _state = state;
        EditSelectedCommand = new RelayCommand(OnEditSelected, CanModify);
        DeleteSelectedCommand = new RelayCommand(async () => await OnDeleteSelected(), CanModify);
        CloseDetailsCommand = new RelayCommand(() => IsEditing = false);
    }

    protected override void SelectedItemChanged(T? value)
    {
        base.SelectedItemChanged(value);
        (EditSelectedCommand as RelayCommand)?.NotifyCanExecuteChanged();
        (DeleteSelectedCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private void OnEditSelected() => IsEditing = !IsEditing;

    private async Task OnDeleteSelected()
    {
        if (SelectedItem != null)
        {
            await DeleteAsync();
            await LoadAsync();
        }
    }

    protected virtual Task DeleteAsync() => Task.CompletedTask;

    private bool CanModify() => SelectedItem != null;

    partial void OnIsEditingChanged(bool value)
        => _state.InteractionState = value
            ? AppInteractionState.EditingMasterData
            : AppInteractionState.BrowsingInvoices;
}
