using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class UnitMasterViewModel : EditableMasterDataViewModel<Unit>
{
    public ObservableCollection<Unit> Units => Items;

    private readonly IUnitService _service;

    public UnitMasterViewModel(IUnitService service)
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
