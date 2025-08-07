using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface ISettingsService
{
    Task<ApplicationSettings> LoadAsync();
    Task SaveAsync(ApplicationSettings settings);
}
