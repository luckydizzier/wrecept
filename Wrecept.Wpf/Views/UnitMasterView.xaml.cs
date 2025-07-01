using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class UnitMasterView : BaseMasterView<UnitMasterViewModel>
{
    public UnitMasterView() : this(App.Provider.GetRequiredService<UnitMasterViewModel>())
    {
    }

    public UnitMasterView(UnitMasterViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
