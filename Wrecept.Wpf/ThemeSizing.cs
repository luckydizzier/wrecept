using System.Windows;
using Wrecept.Core;

namespace Wrecept.Wpf;

public static class ThemeSizing
{
    public static void Apply(ScreenMode mode)
    {
        var res = Application.Current.Resources;
        switch (mode)
        {
            case ScreenMode.Small:
                res["FontSizeNormal"] = 12d;
                res["FontSizeLarge"] = 14d;
                break;
            case ScreenMode.Medium:
                res["FontSizeNormal"] = 14d;
                res["FontSizeLarge"] = 16d;
                break;
            case ScreenMode.Large:
                res["FontSizeNormal"] = 16d;
                res["FontSizeLarge"] = 18d;
                break;
            case ScreenMode.ExtraLarge:
                res["FontSizeNormal"] = 18d;
                res["FontSizeLarge"] = 20d;
                break;
        }
    }
}
