using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductGroupMasterViewModel : MasterDataBaseViewModel<ProductGroup>
{
    public ObservableCollection<ProductGroup> ProductGroups => Items;

    private readonly IProductGroupService _service;

    public ProductGroupMasterViewModel(IProductGroupService service)
    {
        _service = service;
    }

    protected override Task<List<ProductGroup>> GetItemsAsync()
        => _service.GetActiveAsync();
}
