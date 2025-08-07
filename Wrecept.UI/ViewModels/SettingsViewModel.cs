using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.UI.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly ISettingsService _settingsService;
    private ApplicationSettings _settings;

    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _settings = new ApplicationSettings();
        _settingsService.SettingsChanged += (_, s) => { _settings = s; OnPropertyChanged(nameof(Theme)); OnPropertyChanged(nameof(Language)); };
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
                _ = _settingsService.UpdateThemeAsync(value);
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
                _ = _settingsService.UpdateLanguageAsync(value);
            }
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
