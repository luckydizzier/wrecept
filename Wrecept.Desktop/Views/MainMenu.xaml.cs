using System.Windows.Controls;
namespace Wrecept.Desktop.Views;

public partial class MainMenu : UserControl
{
    public Button FirstButton => FirstButtonElement;

    public MainMenu()
    {
        InitializeComponent();
    }
}
