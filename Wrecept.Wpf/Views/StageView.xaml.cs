using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;
using Wrecept.Core.Enums;
using FocusManager = Wrecept.Wpf.Services.FocusManager;

namespace Wrecept.Wpf.Views;

public partial class StageView : UserControl
{
    private readonly StageViewModel _viewModel;
    private MenuItem? _lastMenuItem;
    private readonly KeyboardManager _keyboard;
    private readonly FocusManager _focus;
    private readonly AppStateService _state;

    public StageView(StageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
        _focus = App.Provider.GetRequiredService<FocusManager>();
        _state = App.Provider.GetRequiredService<AppStateService>();
        Keyboard.AddGotKeyboardFocusHandler(this, OnGotKeyboardFocus);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_state.Current is AppState.PromptActive or AppState.Saving or AppState.Error)
        {
            e.Handled = true;
            return;
        }
        if (e.Key == Key.Escape)
        {
            if (_lastMenuItem is not null)
            {
                _focus.RequestFocus(_lastMenuItem);
            }
            else if (Menu.Items.Count > 0)
            {
                if (Menu.Items[0] is MenuItem first)
                    _focus.RequestFocus(first);
            }

            e.Handled = true;
            return;
        }

        if (e.Key is Key.Up or Key.Down && e.OriginalSource == this)
        {
            var last = _focus.GetLast("StageView");
            if (last is not null && !ReferenceEquals(last, this))
            {
                _focus.RequestFocus(last);
                e.Handled = true;
                return;
            }
        }

        if (_state.Current is AppState.Browsing or AppState.Editing)
        {
            _keyboard.Handle(e);
        }
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
        _focus.Update("StageView", e.NewFocus);
        _viewModel.StatusBar.FocusedElement = fe?.Name ?? fe?.GetType().Name ?? string.Empty;
        _viewModel.StatusBar.Message = $"State: {_state.Current}";
    }
}
