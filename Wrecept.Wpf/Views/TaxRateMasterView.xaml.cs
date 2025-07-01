using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class TaxRateMasterView : BaseMasterView<TaxRateMasterViewModel>
{
    public TaxRateMasterView()
    {
    }

    public TaxRateMasterView(TaxRateMasterViewModel viewModel) : base(viewModel)
    {
    }
}
