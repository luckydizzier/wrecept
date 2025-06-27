using System.Windows.Controls;
using Wrecept.Desktop.ViewModels;

namespace Wrecept.Desktop.Views;

public partial class StageView : UserControl
{
    public StageViewModel ViewModel { get; }

    public MainMenuViewModel MenuViewModel => ViewModel.Menu;

    public StageView()
    {
        InitializeComponent();
        ViewModel = new StageViewModel();
        DataContext = ViewModel;
    }
}
