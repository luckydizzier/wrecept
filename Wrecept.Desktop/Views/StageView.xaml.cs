using System.Windows.Controls;
using Wrecept.Desktop.ViewModels;

namespace Wrecept.Desktop.Views;

public partial class StageView : UserControl
{
    public StageViewModel ViewModel
    {
        get => (StageViewModel)DataContext!;
        set => DataContext = value;
    }

    public void FocusMainMenu()
    {
        MainMenuBox.Focus();
    }


    public StageView()
    {
        InitializeComponent();
    }

}
