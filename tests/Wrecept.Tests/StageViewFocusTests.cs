using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Xunit;

namespace Wrecept.Tests;

public class StageViewFocusTests
{
    private class FakeSource : System.Windows.PresentationSource
    {
        public override System.Windows.Media.Visual? RootVisual { get => null; set { } }
        public override bool IsDisposed => false;
        protected override System.Windows.Media.CompositionTarget? GetCompositionTargetCore() => null;
    }

    private static StageView CreateView(out MenuItem first, out MenuItem second)
    {
        var status = new StatusBarViewModel();
        var vm = (StageViewModel)FormatterServices.GetUninitializedObject(typeof(StageViewModel));
        typeof(StageViewModel).GetField("_statusBar", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, status);

        var view = (StageView)FormatterServices.GetUninitializedObject(typeof(StageView));
        typeof(StageView).GetField("_viewModel", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(view, vm);
        typeof(StageView).GetField("_tracker", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(view, new FocusTrackerService());
        typeof(StageView).GetField("_keyboard", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(view, new KeyboardManager());
        typeof(StageView).GetField("_focus", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(view, new FocusManager());

        first = new MenuItem { Header = "A" };
        second = new MenuItem { Header = "B" };
        return view;
    }

    [Fact]
    public void EscapeRestoresLastMenuItemFocus()
    {
        var view = CreateView(out var first, out var second);
        var type = view.GetType();

        var focusMethod = type.GetMethod("OnGotKeyboardFocus", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var keyMethod = type.GetMethod("OnKeyDown", BindingFlags.Instance | BindingFlags.NonPublic)!;

        var args1 = new KeyboardFocusChangedEventArgs(Keyboard.PrimaryDevice, 0, null, first) { RoutedEvent = Keyboard.GotKeyboardFocusEvent };
        focusMethod.Invoke(view, new object[] { view, args1 });

        var args2 = new KeyboardFocusChangedEventArgs(Keyboard.PrimaryDevice, 0, first, second) { RoutedEvent = Keyboard.GotKeyboardFocusEvent };
        focusMethod.Invoke(view, new object[] { view, args2 });

        var escape = new KeyEventArgs(Keyboard.PrimaryDevice, new FakeSource(), 0, Key.Escape)
        {
            RoutedEvent = Keyboard.KeyDownEvent,
            Source = view,
            OriginalSource = view
        };
        keyMethod.Invoke(view, new object[] { view, escape });

        // emulate focus event triggered by Focus() call
        focusMethod.Invoke(view, new object[] { view, args2 });

        var last = typeof(StageView).GetField("_lastMenuItem", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(view);
        Assert.True(escape.Handled);
        Assert.Same(second, last);
    }

    [Fact]
    public void ArrowKeyRestoresLastFocusedElement()
    {
        var view = CreateView(out _, out _);
        var type = view.GetType();

        var focusMethod = type.GetMethod("OnGotKeyboardFocus", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var keyMethod = type.GetMethod("OnKeyDown", BindingFlags.Instance | BindingFlags.NonPublic)!;

        var box = new TextBox();
        var focusArgs = new KeyboardFocusChangedEventArgs(Keyboard.PrimaryDevice, 0, null, box) { RoutedEvent = Keyboard.GotKeyboardFocusEvent };
        focusMethod.Invoke(view, new object[] { view, focusArgs });

        var down = new KeyEventArgs(Keyboard.PrimaryDevice, new FakeSource(), 0, Key.Down)
        {
            RoutedEvent = Keyboard.KeyDownEvent,
            Source = view,
            OriginalSource = view
        };

        keyMethod.Invoke(view, new object[] { view, down });

        Assert.True(down.Handled);
    }
}
