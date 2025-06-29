using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class StageView : UserControl
{
    private readonly StageViewModel _viewModel;

    public StageView(StageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        FocusManager.AddGotFocusHandler(this, OnGotFocus);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);

    private void OnGotFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        var fe = e.NewFocus as FrameworkElement;
        _viewModel.StatusBar.FocusedElement = fe?.Name ?? fe?.GetType().Name ?? string.Empty;
    }
}
