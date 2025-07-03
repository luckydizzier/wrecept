using System.Windows.Controls;
using System.Windows.Input;
using Xunit;
using Wrecept.Wpf;

namespace Wrecept.Tests.ViewModels;

public class NavigationHelperTests
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

        NavigationHelper.Handle(args);

        Assert.False(args.Handled);
    }
}
