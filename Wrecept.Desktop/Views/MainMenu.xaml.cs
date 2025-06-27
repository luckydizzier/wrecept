using System.Windows.Controls;
using Wrecept.Desktop.ViewModels;

namespace Wrecept.Desktop.Views;

public partial class MainMenu : UserControl
{
    public MainMenuViewModel ViewModel { get; }

    public MainMenu()
    {
        InitializeComponent();
        ViewModel = new MainMenuViewModel();
        DataContext = ViewModel;
    }
}
