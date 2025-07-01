using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class SupplierMasterView : BaseMasterView<SupplierMasterViewModel>
{
    public SupplierMasterView()
    {
    }

    public SupplierMasterView(SupplierMasterViewModel viewModel) : base(viewModel)
    {
    }
}
