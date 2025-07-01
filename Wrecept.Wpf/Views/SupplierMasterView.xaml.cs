using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class SupplierMasterView : BaseMasterView<SupplierMasterViewModel>
{
    public SupplierMasterView() : this(App.Provider.GetRequiredService<SupplierMasterViewModel>())
    {
    }

    public SupplierMasterView(SupplierMasterViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
