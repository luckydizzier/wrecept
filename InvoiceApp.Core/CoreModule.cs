using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace InvoiceApp.Core;

public static class CoreModule
{
    public static Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null)
    {
        services.AddCore();
        return Task.CompletedTask;
    }
}
