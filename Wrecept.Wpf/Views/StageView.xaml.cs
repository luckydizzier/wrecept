using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views;

public partial class StageView : UserControl
{
    private readonly StageViewModel _viewModel;
    private MenuItem? _lastMenuItem;
    private readonly IFocusTrackerService _tracker;
    private readonly KeyboardManager _keyboard;
    private readonly FocusManager _focus;

    public StageView(StageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        _tracker = App.Provider.GetRequiredService<IFocusTrackerService>();
        _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
        _focus = App.Provider.GetRequiredService<FocusManager>();
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
                _keyboard.Handle(e);
            }

            return;
        }

        if (e.Key is Key.Up or Key.Down && e.OriginalSource == this)
        {
            var last = _tracker.GetLast("StageView") as IInputElement;
            if (last is not null && !ReferenceEquals(last, this))
            {
                _focus.RequestFocus(last);
                e.Handled = true;
                return;
            }
        }

        _keyboard.Handle(e);
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
