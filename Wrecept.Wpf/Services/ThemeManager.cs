using System;
using System.Windows;

namespace Wrecept.Wpf.Services;

public static class ThemeManager
{
    private static ResourceDictionary? _current;

    public static void ApplyDarkTheme(bool dark)
    {
        var dictionaries = Application.Current.Resources.MergedDictionaries;
        if (_current != null)
        {
            dictionaries.Remove(_current);
        }
        var name = dark ? "RetroTheme.Dark.xaml" : "RetroTheme.xaml";
        var dict = new ResourceDictionary { Source = new Uri($"Themes/{name}", UriKind.Relative) };
        dictionaries.Insert(0, dict);
        _current = dict;
    }
}

