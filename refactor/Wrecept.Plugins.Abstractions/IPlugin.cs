using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Plugins.Abstractions;

public interface IPlugin
{
    Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null);
}
