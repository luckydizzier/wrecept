using System.Windows;
using System.Windows.Input;

namespace Wrecept.Desktop;

public partial class MainWindow : Window
{
    public ViewModels.MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        this.AddHandler(Keyboard.KeyDownEvent, new KeyEventHandler(Window_KeyDown), true);
        ViewModel = new ViewModels.MainWindowViewModel(Stage.ViewModel);
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
            case System.Windows.Input.Key.Up:
                ViewModel.MoveUpCommand.Execute(null);
                break;
            case System.Windows.Input.Key.Down:
                ViewModel.MoveDownCommand.Execute(null);
                break;
            case System.Windows.Input.Key.Enter:
                ViewModel.EnterCommand.Execute(null);
                break;
            case System.Windows.Input.Key.Escape:
                ViewModel.EscapeCommand.Execute(null);
                break;
        }
    }
}
