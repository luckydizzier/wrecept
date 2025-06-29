using System.Windows;
using Wrecept.Wpf.Views;

namespace Wrecept.Wpf;

public partial class MainWindow : Window
{
    public MainWindow(StageView stageView)
    {
        InitializeComponent();
        ContentHost.Content = stageView;
    }
}
