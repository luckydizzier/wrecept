using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;
using FocusService = Wrecept.Wpf.Services.FocusManager;

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
            var focus = App.Provider.GetRequiredService<FocusService>();
            var next = (e.OriginalSource as UIElement)?.PredictFocus(FocusNavigationDirection.Previous);
            focus.RequestFocus(next as IInputElement);
            e.Handled = true;
            return;
        }

        _keyboard?.Handle(e);
    }
}
