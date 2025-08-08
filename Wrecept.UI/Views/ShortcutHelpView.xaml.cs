using System.Windows;

namespace Wrecept.UI.Views;

public partial class ShortcutHelpView : Window
{
    public ShortcutHelpView()
    {
        InitializeComponent();
        DataContext = new ViewModels.ShortcutHelpViewModel(this);
    }
}
