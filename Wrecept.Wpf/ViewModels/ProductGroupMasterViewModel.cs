using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductGroupMasterViewModel : EditableMasterDataViewModel<ProductGroup>
{
    public ObservableCollection<ProductGroup> ProductGroups => Items;

    private readonly IProductGroupService _service;

public ProductGroupMasterViewModel(IProductGroupService service, AppStateService state)
    : base(state)
{
    _service = service;
}

    protected override Task<List<ProductGroup>> GetItemsAsync()
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
