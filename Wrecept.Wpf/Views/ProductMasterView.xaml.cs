using System.Windows;
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
        Loaded += async (_, _) =>
        {
            await viewModel.LoadAsync();
            Grid.Focus();
        };
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);

    private void Grid_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
    {
        if (e.DetailsElement.FindName("NameBox") is TextBox box && e.Row.DetailsVisibility == Visibility.Visible)
        {
            box.Focus();
        }
        else if (e.Row.DetailsVisibility != Visibility.Visible)
        {
            Grid.Focus();
        }
    }
}
