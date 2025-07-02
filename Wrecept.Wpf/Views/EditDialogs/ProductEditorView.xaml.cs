using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.EditDialogs;

public partial class ProductEditorView : UserControl
{
    public ProductEditorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
