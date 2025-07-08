using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Plugins.Abstractions;
using Wrecept.Core;
using Wrecept.Storage;

namespace Wrecept.InvoiceModule;

public class InvoiceModule : IPlugin
{
    public async Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null)
    {
        var dbPath = context?["DbPath"] as string ?? string.Empty;
        var userInfoPath = context?["UserInfoPath"] as string ?? string.Empty;
        var settingsPath = context?["SettingsPath"] as string ?? string.Empty;

        services.AddCore();
        await services.AddStorageAsync(dbPath, userInfoPath, settingsPath);
    }
}
