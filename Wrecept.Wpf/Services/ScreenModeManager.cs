using Wrecept.Core;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.Services;

public class ScreenModeManager
{
    private readonly ISettingsService _settings;

    public ScreenModeManager(ISettingsService settings)
    {
        _settings = settings;
    }

    public ScreenMode CurrentMode { get; private set; } = ScreenMode.Large;

    public async Task ApplySavedAsync(MainWindow window)
    {
        var config = await _settings.LoadAsync();
        CurrentMode = config.ScreenMode;
        Apply(window, CurrentMode);
    }

    public async Task ChangeModeAsync(MainWindow window, ScreenMode mode)
    {
        if (mode == CurrentMode) return;
        CurrentMode = mode;
        Apply(window, mode);
        await _settings.SaveAsync(new AppSettings { ScreenMode = mode });
    }

    private static void Apply(MainWindow window, ScreenMode mode)
    {
        ThemeSizing.Apply(mode);
        var (w, h) = mode switch
        {
            ScreenMode.Small => (800d, 600d),
            ScreenMode.Medium => (1024d, 768d),
            ScreenMode.Large => (1280d, 1024d),
            ScreenMode.ExtraLarge => (1920d, 1080d),
            _ => (1280d, 1024d)
        };
        window.Width = w;
        window.Height = h;
    }
}
