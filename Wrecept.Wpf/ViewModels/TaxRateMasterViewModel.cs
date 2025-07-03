using System;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class TaxRateMasterViewModel : EditableMasterDataViewModel<TaxRate>
{
    public ObservableCollection<TaxRate> TaxRates => Items;

    private readonly ITaxRateService _service;

    public new IRelayCommand EditSelectedCommand { get; }

    public TaxRateMasterViewModel(ITaxRateService service)
    {
        _service = service;
        EditSelectedCommand = new RelayCommand(EditSelected, () => SelectedItem != null);
    }

    protected override Task<List<TaxRate>> GetItemsAsync()
        => _service.GetActiveAsync(DateTime.UtcNow);

    protected override async Task DeleteAsync()
    {
        if (SelectedItem != null)
        {
            SelectedItem.IsArchived = true;
            await _service.UpdateAsync(SelectedItem);
        }
    }

    private async void EditSelected()
    {
        if (SelectedItem is null)
            return;

        var vm = new VatKeyEditorViewModel
        {
            Name = SelectedItem.Name,
            Percentage = SelectedItem.Percentage
        };

        vm.OnOk = async m =>
        {
            SelectedItem.Name = m.Name;
            SelectedItem.Percentage = m.Percentage;
            await _service.UpdateAsync(SelectedItem);
            await LoadAsync();
        };

        DialogService.EditEntity<Views.EditDialogs.VatKeyEditorView, VatKeyEditorViewModel>(
            vm, vm.OkCommand, vm.CancelCommand);
    }
}
