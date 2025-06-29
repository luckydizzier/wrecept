using System.Windows;
using System.Windows.Input;

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

}
