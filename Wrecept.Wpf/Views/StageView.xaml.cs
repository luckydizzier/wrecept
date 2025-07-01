using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class StageView : UserControl
{
    private readonly StageViewModel _viewModel;
    private MenuItem? _lastMenuItem;

    public StageView(StageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        Keyboard.AddGotKeyboardFocusHandler(this, OnGotKeyboardFocus);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            _lastMenuItem?.Focus();
            e.Handled = true;
            return;
        }

        NavigationHelper.Handle(e);
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        _lastMenuItem = sender as MenuItem;
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        var fe = e.NewFocus as FrameworkElement;
        _viewModel.StatusBar.FocusedElement = fe?.Name ?? fe?.GetType().Name ?? string.Empty;
    }
}
