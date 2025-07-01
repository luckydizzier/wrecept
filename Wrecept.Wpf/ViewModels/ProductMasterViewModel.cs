using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductMasterViewModel : ObservableObject
{
    public ObservableCollection<Product> Products { get; } = new();

    private readonly IProductService _service;

    [ObservableProperty]
    private Product? selectedProduct;

    [ObservableProperty]
    private bool isEditing;

    public IRelayCommand EditSelectedCommand { get; }
    public IRelayCommand DeleteSelectedCommand { get; }
    public IRelayCommand CloseDetailsCommand { get; }

    public ProductMasterViewModel(IProductService service)
    {
        _service = service;
        EditSelectedCommand = new RelayCommand(() => IsEditing = !IsEditing, () => SelectedProduct != null);
        DeleteSelectedCommand = new RelayCommand(async () =>
        {
            if (SelectedProduct != null)
            {
                SelectedProduct.IsArchived = true;
                await _service.UpdateAsync(SelectedProduct);
                await LoadAsync();
            }
        }, () => SelectedProduct != null);
        CloseDetailsCommand = new RelayCommand(() => IsEditing = false);
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetActiveAsync();
        Products.Clear();
        foreach (var item in items)
            Products.Add(item);
    }
}
