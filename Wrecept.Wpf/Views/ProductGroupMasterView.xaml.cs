using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class ProductGroupMasterView : BaseMasterView<ProductGroupMasterViewModel>
{
    public ProductGroupMasterView()
    {
    }

    public ProductGroupMasterView(ProductGroupMasterViewModel viewModel) : base(viewModel)
    {
    }
}
