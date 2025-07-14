using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace InvoiceApp.Data;

public static class StorageModule
{
    public static async Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null)
    {
        var dbPath = context?[("DbPath")] as string ?? string.Empty;
        var userInfoPath = context?[("UserInfoPath")] as string ?? string.Empty;
        var settingsPath = context?[("SettingsPath")] as string ?? string.Empty;
        await services.AddStorageAsync(dbPath, userInfoPath, settingsPath);
    }
}
