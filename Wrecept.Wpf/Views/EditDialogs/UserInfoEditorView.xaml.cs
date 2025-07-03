using System.Windows;
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
        if (e.Key == Key.Escape)
        {
            e.Handled = true;
            (e.OriginalSource as UIElement)?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            return;
        }

        _keyboard?.Handle(e);
    }
}
