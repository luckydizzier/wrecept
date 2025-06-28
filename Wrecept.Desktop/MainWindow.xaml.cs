using System.Windows;

namespace Wrecept.Desktop;

public partial class MainWindow : Window
{
    public ViewModels.MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new ViewModels.MainWindowViewModel();
        DataContext = ViewModel;
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case System.Windows.Input.Key.Left:
                ViewModel.MoveLeftCommand.Execute(null);
                break;
            case System.Windows.Input.Key.Right:
                ViewModel.MoveRightCommand.Execute(null);
                break;
            case System.Windows.Input.Key.Enter:
                ViewModel.EnterCommand.Execute(null);
                break;
        }
    }
}
