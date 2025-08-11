using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.UI.Services;

namespace Wrecept.UI.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly ISettingsService _settingsService;
    private readonly IMessageService _messageService;
    private ApplicationSettings _settings;

    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsViewModel(ISettingsService settingsService, IMessageService messageService)
    {
        _settingsService = settingsService;
        _messageService = messageService;
        _settings = new ApplicationSettings();
        _settingsService.SettingsChanged += (_, s) =>
        {
            _settings = s;
            OnPropertyChanged(nameof(Theme));
            OnPropertyChanged(nameof(Language));
        };
    }

    public string Theme
    {
        get => _settings.Theme;
        set
        {
            if (_settings.Theme != value)
            {
                _settings.Theme = value;
                OnPropertyChanged();
                _ = SafeUpdateThemeAsync(value);
            }
        }
    }

    public string Language
    {
        get => _settings.Language;
        set
        {
            if (_settings.Language != value)
            {
                _settings.Language = value;
                OnPropertyChanged();
                _ = SafeUpdateLanguageAsync(value);
            }
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private async Task SafeUpdateThemeAsync(string theme)
    {
        try
        {
            await _settingsService.UpdateThemeAsync(theme);
        }
        catch (Exception ex)
        {
            _messageService.Show($"Téma mentése sikertelen: {ex.Message}", "Hiba");
        }
    }

    private async Task SafeUpdateLanguageAsync(string language)
    {
        try
        {
            await _settingsService.UpdateLanguageAsync(language);
        }
        catch (Exception ex)
        {
            _messageService.Show($"Nyelv mentése sikertelen: {ex.Message}", "Hiba");
        }
    }
}
