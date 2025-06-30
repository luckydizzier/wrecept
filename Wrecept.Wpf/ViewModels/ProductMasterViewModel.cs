using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductMasterViewModel : ObservableObject
{
    public ObservableCollection<Product> Products { get; } = new();

    private readonly IProductService _service;

    public ProductMasterViewModel(IProductService service)
    {
        _service = service;
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetActiveAsync();
        Products.Clear();
        foreach (var item in items)
            Products.Add(item);
    }
}
