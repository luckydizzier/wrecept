using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views.Controls;

public abstract class BaseMasterView<TViewModel> : BaseMasterView
    where TViewModel : class, IMasterDataViewModel
{
    protected BaseMasterView() : this(App.Provider.GetRequiredService<TViewModel>())
    {
    }

    protected BaseMasterView(TViewModel viewModel)
    {
        InitializeViewModel(viewModel);
    }
}
