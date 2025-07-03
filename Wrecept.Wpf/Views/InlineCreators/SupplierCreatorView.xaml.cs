using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views.InlineCreators;

public partial class SupplierCreatorView : UserControl
{
    private readonly KeyboardManager _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
    public SupplierCreatorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => _keyboard.Handle(e);
}
