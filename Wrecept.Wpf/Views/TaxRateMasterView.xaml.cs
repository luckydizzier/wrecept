using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class TaxRateMasterView : BaseMasterView<TaxRateMasterViewModel>
{
    public TaxRateMasterView() : this(App.Provider.GetRequiredService<TaxRateMasterViewModel>())
    {
    }

    public TaxRateMasterView(TaxRateMasterViewModel viewModel)
    {
        InitializeComponent();
        InitializeViewModel(viewModel);
    }
}
