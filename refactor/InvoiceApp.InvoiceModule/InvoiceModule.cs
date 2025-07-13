using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using InvoiceApp.Plugins.Abstractions;
using InvoiceApp.Core;
using InvoiceApp.Data;

namespace InvoiceApp.InvoiceModule;

public class InvoiceModule : IPlugin
{
    private const string DbPathKey = "DbPath";
    private const string UserInfoPathKey = "UserInfoPath";
    private const string SettingsPathKey = "SettingsPath";

    public async Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null)
    {
        var dbPath = context?[DbPathKey] as string ?? string.Empty;
        var userInfoPath = context?[UserInfoPathKey] as string ?? string.Empty;
        var settingsPath = context?[SettingsPathKey] as string ?? string.Empty;

        services.AddCore();
        await services.AddStorageAsync(dbPath, userInfoPath, settingsPath);
    }
}
