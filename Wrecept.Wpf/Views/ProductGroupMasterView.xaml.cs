using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class ProductGroupMasterView : UserControl
{
    public ProductGroupMasterView() : this(App.Provider.GetRequiredService<ProductGroupMasterViewModel>())
    {
    }

    public ProductGroupMasterView(ProductGroupMasterViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (_, _) => await viewModel.LoadAsync();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
