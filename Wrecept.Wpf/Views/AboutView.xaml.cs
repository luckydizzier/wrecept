using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views;

public partial class AboutView : UserControl
{
    private readonly KeyboardManager _keyboard = App.Provider.GetRequiredService<KeyboardManager>();

    public AboutView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => _keyboard.Handle(e);
}
