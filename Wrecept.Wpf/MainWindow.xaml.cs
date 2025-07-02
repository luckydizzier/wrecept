using System.Windows;
using Wrecept.Wpf.Views;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf;

public partial class MainWindow : Window
{
    public MainWindow(StageView stageView, ScreenModeManager screenModeManager)
    {
        InitializeComponent();
        ContentHost.Content = stageView;
        Loaded += async (_, _) => await screenModeManager.ApplySavedAsync(this);
    }
}
