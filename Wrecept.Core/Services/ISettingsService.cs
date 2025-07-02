using Wrecept.Core.Entities;

namespace Wrecept.Core.Services;

public interface ISettingsService
{
    Task<AppSettings> LoadAsync();
    Task SaveAsync(AppSettings settings);
}
