using System.Windows;
using System.Windows.Input;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class ScreenModeWindow : Window
{
    private readonly KeyboardManager _keyboard;

    public ScreenModeWindow(ScreenModeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            return;

        _keyboard.Handle(e);
    }
}
