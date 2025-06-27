using System.Windows;

namespace Wrecept.Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        try
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    ((Views.StageView)Content).Menu.ViewModel.MoveUpCommand.Execute(null);
                    break;
                case System.Windows.Input.Key.Down:
                    ((Views.StageView)Content).Menu.ViewModel.MoveDownCommand.Execute(null);
                    break;
                case System.Windows.Input.Key.Enter:
                    ((Views.StageView)Content).Menu.ViewModel.EnterCommand.Execute(null);
                    System.Console.Beep();
                    break;
            }
        }
        catch
        {
            // suppress all runtime exceptions during prototype stage
        }
    }
}
