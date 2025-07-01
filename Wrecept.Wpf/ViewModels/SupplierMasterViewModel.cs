using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wrecept.Wpf.ViewModels;

public partial class SupplierMasterViewModel : MasterDataBaseViewModel<Supplier>
{
    private readonly ISupplierService _service;

    public SupplierMasterViewModel(ISupplierService service)
    {
        _service = service;
    }

    public ObservableCollection<Supplier> Suppliers => Items;

    protected override Task<List<Supplier>> GetItemsAsync()
        => _service.GetActiveAsync();
}
