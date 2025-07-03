using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.InlineCreators;

public partial class UnitCreatorView : UserControl
{
    private readonly KeyboardManager _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
    public UnitCreatorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => _keyboard.Handle(e);
}
