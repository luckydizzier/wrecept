using System;
using System.Diagnostics;
using System.Reflection;

namespace Wrecept.Desktop;

public static class BuildInfo
{
    public static string CommitHash { get; }
    public static string BuildTimestamp { get; } = DateTime.UtcNow.ToString("u");
    public static string Version { get; }

    static BuildInfo()
    {
        CommitHash = GetCommitHash();
        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "N/A";
    }

    private static string GetCommitHash()
    {
        try
        {
            var psi = new ProcessStartInfo("git", "rev-parse HEAD")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            using var p = Process.Start(psi);
            if (p == null) return "N/A";
            var hash = p.StandardOutput.ReadLine();
            p.WaitForExit(1000);
            return hash ?? "N/A";
        }
        catch
        {
            return "N/A";
        }
    }

    public static string GetInfo() => $"Commit: {CommitHash}\nBuilt: {BuildTimestamp}\nVersion: {Version}";
}
