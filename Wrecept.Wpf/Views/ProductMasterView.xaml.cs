using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class ProductMasterView : UserControl
{
    public ProductMasterView() : this(App.Provider.GetRequiredService<ProductMasterViewModel>())
    {
    }

    public ProductMasterView(ProductMasterViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (_, _) => await viewModel.LoadAsync();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
