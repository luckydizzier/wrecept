using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductGroupMasterViewModel : ObservableObject
{
    public ObservableCollection<ProductGroup> ProductGroups { get; } = new();

    private readonly IProductGroupService _service;

    public ProductGroupMasterViewModel(IProductGroupService service)
    {
        _service = service;
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        ProductGroups.Clear();
        foreach (var item in items)
            ProductGroups.Add(item);
    }
}
