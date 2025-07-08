using System.Reflection;

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
                logger.LogError(ex, $"Failed to load plugin {dll}");
            }
        }
    }
}
