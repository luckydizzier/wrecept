using System.Windows;

namespace Wrecept.UI.Views;

public partial class ThemeEditorWindow : Window
{
    public ThemeEditorWindow(ThemeEditorView view)
    {
        InitializeComponent();
        Content = view;
    }
}
