using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class ProductGroupMasterView : BaseMasterView<ProductGroupMasterViewModel>
{
    public ProductGroupMasterView() : this(App.Provider.GetRequiredService<ProductGroupMasterViewModel>())
    {
    }

    public ProductGroupMasterView(ProductGroupMasterViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
