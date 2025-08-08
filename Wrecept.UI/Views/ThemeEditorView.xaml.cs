using System.Windows.Controls;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Views;

public partial class ThemeEditorView : UserControl
{
    public ThemeEditorView()
    {
        InitializeComponent();
        DataContext = new ThemeEditorViewModel();
    }
}
