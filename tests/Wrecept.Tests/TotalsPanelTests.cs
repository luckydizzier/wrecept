using System.Windows;
using Wrecept.Wpf.Views.Controls;
using Xunit;

namespace Wrecept.Tests;

public class TotalsPanelTests
{
    [Fact]
    public void IsCompactMode_RoundTrips()
    {
        var panel = new TotalsPanel();
        panel.IsCompactMode = true;
        Assert.True((bool)panel.GetValue(TotalsPanel.IsCompactModeProperty));
        panel.IsCompactMode = false;
        Assert.False((bool)panel.GetValue(TotalsPanel.IsCompactModeProperty));
    }
}
