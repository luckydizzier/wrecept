using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views.EditDialogs;

public partial class UserInfoEditorView : UserControl
{
    private readonly KeyboardManager? _keyboard = App.Services != null
        ? App.Provider.GetRequiredService<KeyboardManager>()
        : null;

    public UserInfoEditorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        _keyboard?.Handle(e);
    }
}
