using InvoiceApp.Core.Entities;

namespace InvoiceApp.Core.Services;

public interface ISettingsService
{
    Task<AppSettings> LoadAsync();
    Task SaveAsync(AppSettings settings);
}
