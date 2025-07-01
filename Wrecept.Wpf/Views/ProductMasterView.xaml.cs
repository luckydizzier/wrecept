using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class ProductMasterView : BaseMasterView<ProductMasterViewModel>
{
    public ProductMasterView() : this(App.Provider.GetRequiredService<ProductMasterViewModel>())
    {
    }

    public ProductMasterView(ProductMasterViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
