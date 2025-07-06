using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Wrecept.Core.Enums;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class FocusManagerTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void RequestFocus_FocusesElement()
    {
        EnsureApp();
        var state = new AppStateService("x");
        var manager = new FocusManager(state);
        var window = new Window();
        var box = new TextBox();
        window.Content = box;
        window.Show();

        manager.RequestFocus(box);
        box.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);

        Assert.True(box.IsKeyboardFocused);
        window.Close();
    }

    [StaFact]
    public void InteractionStateChange_TriggersFocus()
    {
        EnsureApp();
        var state = new AppStateService("x");
        var manager = new FocusManager(state);
        var window = new Window();
        var box = new TextBox();
        window.Content = box;
        window.Show();

        manager.Register(AppInteractionState.MainMenu, () => box);
        state.InteractionState = AppInteractionState.MainMenu;
        box.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);

        Assert.True(box.IsKeyboardFocused);
        window.Close();
    }
}
