using System;
using System.Linq;
using System.Windows;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class ThemeManagerTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
        Application.Current.Resources.MergedDictionaries.Clear();
    }

    [StaFact]
    public void ApplyDarkTheme_UpdatesDictionary()
    {
        EnsureApp();

        ThemeManager.ApplyDarkTheme(false);
        var dict = Application.Current.Resources.MergedDictionaries.First();
        Assert.EndsWith("RetroTheme.xaml", dict.Source!.OriginalString);

        ThemeManager.ApplyDarkTheme(true);
        dict = Application.Current.Resources.MergedDictionaries.First();
        Assert.EndsWith("RetroTheme.Dark.xaml", dict.Source!.OriginalString);
    }
}
