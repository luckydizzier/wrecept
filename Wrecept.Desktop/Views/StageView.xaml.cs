using System.Windows.Controls;
using Wrecept.Desktop.ViewModels;

namespace Wrecept.Desktop.Views;

public partial class StageView : UserControl
{
    public StageViewModel ViewModel { get; }


    public StageView()
    {
        InitializeComponent();
        ViewModel = new StageViewModel();
        DataContext = ViewModel;
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        MainMenuFirstButton.Focus();
    }
}
