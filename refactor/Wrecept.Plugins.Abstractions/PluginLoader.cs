using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Plugins.Abstractions;

public static class PluginLoader
{
    public static async Task LoadPluginsAsync(IServiceCollection services, string pluginsPath, IDictionary<string, object>? context = null)
    {
        if (!Directory.Exists(pluginsPath))
            return;

        foreach (var dll in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

                foreach (var type in pluginTypes)
                {
                    if (Activator.CreateInstance(type) is IPlugin plugin)
                    {
                        await plugin.ConfigureServicesAsync(services, context);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load plugin {dll}: {ex.Message}");
            }
        }
    }
}
