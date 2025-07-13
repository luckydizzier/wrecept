using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using InvoiceApp.MAUI.Services;

namespace InvoiceApp.MAUI.ViewModels;

public partial class UnitMasterViewModel : EditableMasterDataViewModel<Unit>
{
    public ObservableCollection<Unit> Units => Items;

    private readonly IUnitService _service;

    public UnitMasterViewModel(IUnitService service, AppStateService state)
        : base(state)
    {
        _service = service;
    }

    protected override Task<List<Unit>> GetItemsAsync()
        => _service.GetActiveAsync();

    protected override async Task DeleteAsync()
    {
        if (SelectedItem != null)
        {
            SelectedItem.IsArchived = true;
            await _service.UpdateAsync(SelectedItem);
        }
    }
}
