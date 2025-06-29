using System.IO;
using System.Reflection;

namespace Wrecept.Desktop;

public static class BuildInfo
{
    public static string Version { get; }
    public static string CommitHash { get; }
    public static DateTime BuildTime { get; }

    static BuildInfo()
    {
        var asm = Assembly.GetExecutingAssembly();
        Version = asm.GetName().Version?.ToString() ?? "0.0";
        var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        CommitHash = info?.Split('+').LastOrDefault() ?? "unknown";
        BuildTime = File.GetLastWriteTimeUtc(asm.Location);
    }
}
