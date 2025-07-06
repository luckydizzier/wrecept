using System.Windows;
using Wrecept.Core;
using Wrecept.Wpf;
using Xunit;

namespace Wrecept.Tests;

public class ThemeSizingTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    private static void ResetResources()
    {
        var res = Application.Current.Resources;
        res["FontSizeNormal"] = 0d;
        res["FontSizeLarge"] = 0d;
    }

    [StaFact]
    public void Apply_Small_SetsSmallSizes()
    {
        EnsureApp();
        ResetResources();

        ThemeSizing.Apply(ScreenMode.Small);

        Assert.Equal(12d, Application.Current.Resources["FontSizeNormal"]);
        Assert.Equal(14d, Application.Current.Resources["FontSizeLarge"]);
    }

    [StaFact]
    public void Apply_Medium_SetsMediumSizes()
    {
        EnsureApp();
        ResetResources();

        ThemeSizing.Apply(ScreenMode.Medium);

        Assert.Equal(14d, Application.Current.Resources["FontSizeNormal"]);
        Assert.Equal(16d, Application.Current.Resources["FontSizeLarge"]);
    }

    [StaFact]
    public void Apply_Large_SetsLargeSizes()
    {
        EnsureApp();
        ResetResources();

        ThemeSizing.Apply(ScreenMode.Large);

        Assert.Equal(16d, Application.Current.Resources["FontSizeNormal"]);
        Assert.Equal(18d, Application.Current.Resources["FontSizeLarge"]);
    }

    [StaFact]
    public void Apply_ExtraLarge_SetsExtraLargeSizes()
    {
        EnsureApp();
        ResetResources();

        ThemeSizing.Apply(ScreenMode.ExtraLarge);

        Assert.Equal(18d, Application.Current.Resources["FontSizeNormal"]);
        Assert.Equal(20d, Application.Current.Resources["FontSizeLarge"]);
    }
}
