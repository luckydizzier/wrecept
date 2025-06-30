using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class SupplierMasterView : UserControl
{
    public SupplierMasterView(SupplierMasterViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (_, _) => await viewModel.LoadAsync();
    }

    public static SupplierMasterView Create()
        => App.Provider.GetRequiredService<SupplierMasterView>();

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
