using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.EditDialogs;

public partial class ProductEditorView : UserControl
{
    private readonly KeyboardManager _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
    public ProductEditorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => _keyboard.Handle(e);
}
