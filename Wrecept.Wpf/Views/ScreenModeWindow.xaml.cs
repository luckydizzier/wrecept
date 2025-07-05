using System.Windows;
using System.Windows.Input;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Wpf.Views;

public partial class ScreenModeWindow : Window
{
    public ScreenModeWindow(ScreenModeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
