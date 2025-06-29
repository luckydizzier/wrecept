using System.Windows;

namespace Wrecept.Desktop;

public partial class MainWindow : Window
{
    public ViewModels.MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new ViewModels.MainWindowViewModel(Stage.ViewModel);
        DataContext = ViewModel;
    }

    // InputBindings a MainWindow.xaml-ben gondoskodik a billentyűkezelésről,
    // ezért a korábbi Window_KeyDown kezelő inaktiválva marad.
    /*
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
    */
}
