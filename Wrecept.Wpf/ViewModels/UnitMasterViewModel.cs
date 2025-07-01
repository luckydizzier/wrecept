using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wrecept.Wpf.ViewModels;

public partial class UnitMasterViewModel : MasterDataBaseViewModel<Unit>
{
    public ObservableCollection<Unit> Units => Items;

    private readonly IUnitService _service;

    public UnitMasterViewModel(IUnitService service)
    {
        _service = service;
    }

    protected override Task<List<Unit>> GetItemsAsync()
        => _service.GetActiveAsync();
}
