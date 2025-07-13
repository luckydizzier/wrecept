using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using InvoiceApp.Plugins.Abstractions;

namespace InvoiceApp.Core;

public class CoreModule : IPlugin
{
    public Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null)
    {
        services.AddCore();
        return Task.CompletedTask;
    }
}
