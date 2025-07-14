using Microsoft.Maui.Controls;
using InvoiceApp.Core;
using InvoiceApp.Core.Entities;
using InvoiceApp.Core.Services;

namespace InvoiceApp.MAUI.Services;

public class ScreenModeManager(ISettingsService settings)
{
    private readonly ISettingsService _settings = settings;
    public ScreenMode CurrentMode { get; private set; } = ScreenMode.Medium;

    public async Task ApplySavedAsync(Window window)
    {
        var s = await _settings.LoadAsync();
        CurrentMode = s.ScreenMode;
        await ChangeModeAsync(window, CurrentMode);
    }

    public async Task ChangeModeAsync(Window window, ScreenMode mode)
    {
        CurrentMode = mode;
        switch (mode)
        {
            case ScreenMode.Small:
                window.Width = 800; window.Height = 600; break;
            case ScreenMode.Medium:
                window.Width = 1024; window.Height = 768; break;
            case ScreenMode.Large:
                window.Width = 1280; window.Height = 1024; break;
            case ScreenMode.ExtraLarge:
                window.Width = 1920; window.Height = 1080; break;
        }
        var s = await _settings.LoadAsync();
        s.ScreenMode = mode;
        await _settings.SaveAsync(s);
    }
}
