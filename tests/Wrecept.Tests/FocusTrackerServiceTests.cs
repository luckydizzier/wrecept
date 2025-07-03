using System.Windows.Controls;
using Xunit;
using Wrecept.Wpf.Services;

namespace Wrecept.Tests;

public class FocusTrackerServiceTests
{
    [Fact]
    public void GetLast_ReturnsNull_WhenNotTracked()
    {
        var tracker = new FocusTrackerService();
        Assert.Null(tracker.GetLast("Any"));
    }

    [Fact]
    public void Update_StoresLastElement_PerView()
    {
        var tracker = new FocusTrackerService();
        var first = new TextBox();
        var second = new TextBox();

        tracker.Update("ViewA", first);
        tracker.Update("ViewA", second);
        tracker.Update("ViewB", first);

        Assert.Same(second, tracker.GetLast("ViewA"));
        Assert.Same(first, tracker.GetLast("ViewB"));
    }
}
