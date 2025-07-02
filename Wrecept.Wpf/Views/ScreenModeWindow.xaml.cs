using System.Windows;
using System.Windows.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class ScreenModeWindow : Window
{
    public ScreenModeWindow(ScreenModeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
