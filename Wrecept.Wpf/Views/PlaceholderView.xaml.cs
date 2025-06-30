using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class PlaceholderView : UserControl
{
    public PlaceholderView() : this(App.Provider.GetRequiredService<PlaceholderViewModel>())
    {
    }

    public PlaceholderView(PlaceholderViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
