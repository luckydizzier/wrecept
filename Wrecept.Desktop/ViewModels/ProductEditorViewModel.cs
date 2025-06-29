using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Services;

namespace Wrecept.Desktop.ViewModels;

public partial class ProductEditorViewModel : ObservableObject
{
    private readonly IProductService _service;

    public ProductEditorViewModel(IProductService service)
    {
        _service = service;
    }
}
