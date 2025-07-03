using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views;

public partial class StageView : UserControl
{
    private readonly StageViewModel _viewModel;
    private MenuItem? _lastMenuItem;
    private readonly IFocusTrackerService _tracker;

    public StageView(StageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        _tracker = App.Provider.GetRequiredService<IFocusTrackerService>();
        Keyboard.AddGotKeyboardFocusHandler(this, OnGotKeyboardFocus);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            if (_lastMenuItem is not null)
            {
                _lastMenuItem.Focus();
                e.Handled = true;
            }
            else
            {
                NavigationHelper.Handle(e);
            }

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
        if (fe is MenuItem menuItem)
        {
            _lastMenuItem = menuItem;
        }
        _tracker.Update("StageView", e.NewFocus);
        _viewModel.StatusBar.FocusedElement = fe?.Name ?? fe?.GetType().Name ?? string.Empty;
    }
}
