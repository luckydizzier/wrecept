using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface ISettingsService
{
    event EventHandler<ApplicationSettings>? SettingsChanged;
    Task<ApplicationSettings> LoadAsync();
    Task SaveAsync(ApplicationSettings settings);
    Task UpdateThemeAsync(string theme);
    Task UpdateLanguageAsync(string language);
}
