using System.Windows.Controls;
using System.Windows.Input;
using Xunit;
using Wrecept.Wpf.Services;

namespace Wrecept.Tests.ViewModels;

public class KeyboardManagerTests
{
    private class FakeSource : System.Windows.PresentationSource
    {
        public override System.Windows.Media.Visual? RootVisual { get => null; set { } }
        public override bool IsDisposed => false;
        protected override System.Windows.Media.CompositionTarget? GetCompositionTargetCore() => null;
    }

    [Fact]
    public void UpArrow_InMenuItem_IsNotHandled()
    {
        var menu = new Menu();
        var item = new MenuItem();
        menu.Items.Add(item);

        var args = new KeyEventArgs(Keyboard.PrimaryDevice, new FakeSource(), 0, Key.Up)
        {
            RoutedEvent = Keyboard.KeyDownEvent,
            Source = item,
            OriginalSource = item
        };

        var manager = new KeyboardManager(new AppStateService());
        manager.Handle(args);

        Assert.False(args.Handled);
    }

    [Fact]
    public void DisabledProfile_DoesNotHandleKeys()
    {
        var box = new TextBox();
        var args = new KeyEventArgs(Keyboard.PrimaryDevice, new FakeSource(), 0, Key.Down)
        {
            RoutedEvent = Keyboard.KeyDownEvent,
            Source = box,
            OriginalSource = box
        };

        var manager = new KeyboardManager(new AppStateService()) { Profile = NavigationProfile.Disabled };
        manager.Handle(args);

        Assert.False(args.Handled);
    }
}
