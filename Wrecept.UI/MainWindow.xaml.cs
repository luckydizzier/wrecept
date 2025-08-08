using System.ComponentModel;
using System.Windows;

namespace Wrecept.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += OnClosing;
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        var confirm = MessageBox.Show(
            Resources.ConfirmExitMessage,
            Resources.ConfirmationTitle,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
        {
            e.Cancel = true;
        }
    }
}
