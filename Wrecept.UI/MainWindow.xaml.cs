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
        string msg = Application.Current.TryFindResource("ConfirmExitMessage") as string
                      ?? "Biztosan kilépsz az alkalmazásból?";
        string title = Application.Current.TryFindResource("ConfirmExitTitle") as string
                        ?? "Kilépés megerősítése";
        var confirm = MessageBox.Show(
            msg,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
        {
            e.Cancel = true;
        }
    }
}
