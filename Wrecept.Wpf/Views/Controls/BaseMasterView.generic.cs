using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views.Controls;

public abstract class BaseMasterView<TViewModel> : BaseMasterView
    where TViewModel : class, IMasterDataViewModel
{
    protected BaseMasterView(TViewModel viewModel)
    {
        InitializeViewModel(viewModel);
    }
}
