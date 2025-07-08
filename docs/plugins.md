# Plug-in architektúra

Ez a dokumentum bemutatja, hogyan bővíthető a Wrecept moduláris plug-inekkel.

## IPlugin interfész

A `refactor/Wrecept.Plugins.Abstractions` projekt tartalmazza az `IPlugin` interfészt és a `PluginLoader` segédosztályt.

```csharp
public interface IPlugin
{
    Task ConfigureServicesAsync(IServiceCollection services, IDictionary<string, object>? context = null);
}
```

A `PluginLoader.LoadPluginsAsync` metódus betölti a megadott mappában található DLL-eket, megkeresi az `IPlugin` implementációkat, majd meghívja azok `ConfigureServicesAsync` metódusát.

## Saját modulok

Minden projekt létrehozhat egy modulosztályt, amely implementálja az `IPlugin` interfészt és regisztrálja a szükséges szolgáltatásokat. Például a `Wrecept.Wpf` projekt a `WpfModule` osztályt tartalmazza.

A host alkalmazás az indításkor betölti a plug-ineket:

```csharp
var context = new Dictionary<string, object>
{
    ["DbPath"] = settings.DatabasePath,
    ["UserInfoPath"] = settings.UserInfoPath,
    ["SettingsPath"] = SettingsPath
};

var module = new WpfModule();
await module.ConfigureServicesAsync(services, context);
await PluginLoader.LoadPluginsAsync(services, Path.Combine(AppContext.BaseDirectory, "plugins"), context);
```

Minden külső plug-int a `plugins` mappába kell másolni. A betöltés sorrendje nincs garantálva.
