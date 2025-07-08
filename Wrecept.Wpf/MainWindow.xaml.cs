using System.Windows;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Wpf;

public partial class MainWindow : Window
{
    public MainWindow(StageView stageView, ScreenModeManager screenModeManager)
    {
        InitializeComponent();
        ContentHost.Content = stageView;
        PreviewKeyDown += OnPreviewKeyDown;
        Loaded += async (_, _) => await screenModeManager.ApplySavedAsync(this);
    }

    private void OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        var km = App.Provider.GetRequiredService<KeyboardManager>();
        if (km.Process(e))
            e.Handled = true;
    }
}
