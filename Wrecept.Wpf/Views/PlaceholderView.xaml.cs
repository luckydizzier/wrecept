using System.Windows.Controls;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class PlaceholderView : UserControl
{
    public PlaceholderView(PlaceholderViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
